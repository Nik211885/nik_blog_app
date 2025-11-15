using System.Security.Claims;
using Application.Adapters;
using Application.Enums;
using Application.Services.NotificationTemplateManager;
using Application.Services.SignInManager;
using Application.Services.SignInManager.Requests;
using Application.Services.UserManager;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Application.Services.SignInManager.Models;
using WebApi.Pipelines.Filters;
using WebApi.Services.BackgroundTaskQueue;
using Application.Entities;
using Application.Services.UserManager.Requests;
using Infrastructure.Data.Extensions;
using Application.Services.UserManager.Models;
using Application.Services.SignInManager.Responses;
using WebApi.Extensions;

namespace WebApi.Controllers
{
    [Route("api/sign-in")]
    [ApiController]
    public class SignInController : ControllerBase
    {
        private readonly ILogger<SignInController> _logger;
        private readonly SignInManagerServices _signInManagerServices;
        private readonly IUserProvider _userProvider;
        private readonly UserManagerServices _userManagerServices;
        private readonly NotificationTemplateServices _notificationTemplateServices;
        private readonly IJwtManagement _jwtManagement;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        public SignInController(ILogger<SignInController> logger, SignInManagerServices signInManagerServices,
        IJwtManagement jwtManagement, UserManagerServices userManagerServices,
        ApplicationDbContext applicationDbContext, NotificationTemplateServices notificationTemplateServices,
        IBackgroundTaskQueue backgroundTaskQueue, IUserProvider userProvider)
        {
            _logger = logger;
            _signInManagerServices = signInManagerServices;
            _jwtManagement = jwtManagement;
            _userManagerServices = userManagerServices;
            _applicationDbContext = applicationDbContext;
            _notificationTemplateServices = notificationTemplateServices;
            _backgroundTaskQueue = backgroundTaskQueue;
            _userProvider = userProvider;
        }
        [HttpPost("login-password")]
        [ValidationFilter]
        public async Task<Results<Ok<JwtResult>, BadRequest, ProblemHttpResult>> PasswordLogin(PasswordLoginRequest request, CancellationToken cancellationToken = default)
        {
            var userSign = await _signInManagerServices.PasswordLoginAsync(request, cancellationToken);
            var claims = MapToJwtPayloadFromUserResponse(userSign);
            var jwtResult = await _jwtManagement.GenerateTokensAsync(userSign.UserName, claims);
            return TypedResults.Ok(jwtResult);
        }
        [HttpGet("login")]
        public IActionResult LoginWithProvider(LoginProviderEx provider, string? returnUrl = "/")
        {
            var redirectUrl = Url.Action("external-login-callback", "SignInController", new { returnUrl });
            var properties = new AuthenticationProperties
            {
                RedirectUri = redirectUrl
            };
            return Challenge(properties, provider.ToString());
        }
        [HttpGet("external-login-callback")]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = "/")
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var userClaims = result.Principal?.Claims;
            var emailClaims = userClaims?.FirstOrDefault(x => x.Type == ClaimTypes.Email);
            var provider = emailClaims?.Issuer ?? throw new Exception("Can't find issuer in claims");
            var email = emailClaims?.Value;
            var identifier = userClaims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("Can't name identifier find  in claims");
            var firstName = userClaims?.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value;
            var lastName = userClaims?.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
            var fullName = userClaims?.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            // find user by email
            string token = string.Empty;
            string redirectUrl = string.Empty;
            var user = await _applicationDbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            // 1. if user exits
            if (user is not null)
            {
                // 1.1 find login provider with provider and identifier
                var loginProvider = await _applicationDbContext.LoginProviders
                            .FirstOrDefaultAsync(x => x.Identifier == identifier && x.Provider.ToString() == provider);
                // 1.1.1 if login provider exits
                // -> return token to get authozization token
                // -> return token to get authozization token 
                // 1.1.2 else
                if(loginProvider is null)
                {
                    var metadataToLinkWithProvider = GetMetadataForLinkAccountToken(provider, identifier);
                    token = _signInManagerServices.GeneratorSignToken(user, SignInTokenType.LinkProvider, metaData: metadataToLinkWithProvider);
                    redirectUrl = $"{returnUrl}?type=link-provider&userId={user.Id}&token={Uri.EscapeDataString(token)}";
                    return Redirect(redirectUrl);
                    // -> return token to confirm link account with new provider  
                }
            }
            // 2.else
            else
            {
                if (email is null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Can not find email address in claims" });
                }
                var userName = await _applicationDbContext.Users.GenerateUniqueUserNameByEmailAddressAsync(email);
                var createUserRequest = new CreateUserRequest()
                {
                    UserName = userName,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    FullName = fullName,
                };
                // In here if email confirm loss after created user success i don't cared because user need try again  when loss action
                //  if you want to data don't open loss you can use outbox or saga or use trainsaction because it just is single databbase
                user = await _userManagerServices.CreateUserAsync(createUserRequest);
                var tokenConfirmEmail = _userManagerServices.GeneratorUserToken(user, UserTokenType.ConfirmEmail);
                await _userManagerServices.ConfirmEmailAsync(user.Id, token);
                var metadataToLinkWithProvider = GetMetadataForLinkAccountToken(provider, identifier);
                string tokenLinkWithProvider = _signInManagerServices.GeneratorSignToken(user, SignInTokenType.LinkProvider, metaData: metadataToLinkWithProvider);
                await _signInManagerServices.LinkWithProviderAsync(user.Id, tokenLinkWithProvider);
                _backgroundTaskQueue.QueueBackgoundWorkItem(async token =>
                {
                    await _notificationTemplateServices.SendMailWithTemplateServiceAsync(
                       NotificationServicesType.CreateNewAccountByExtendProvider,
                       user.Email!,
                       user.FullName, token);
                });
                // create new instance for user and add login with provider 
                // return authozization token to exchane token to login 

            }
            token = _signInManagerServices.GeneratorSignToken(user, SignInTokenType.AuthorizationCode);
            redirectUrl = $"{returnUrl}?type=authozization-code&userId={user.Id}&token={Uri.EscapeDataString(token)}";
            return Redirect(returnUrl);
        }
        [HttpPost("token-exchange")]
        public async Task<Results<Ok<JwtResult>, BadRequest, ProblemHttpResult>> ExchangeToken(Guid userId, string token)
        {
            UserSignResponse userSignResponse = await _signInManagerServices.ValidExchangeTokenAsync(userId, token);
            List<Claim> userClaimInPayload = MapToJwtPayloadFromUserResponse(userSignResponse);
            JwtResult jwtResult = await _jwtManagement.GenerateTokensAsync(userSignResponse.UserName, userClaimInPayload);
            return TypedResults.Ok(jwtResult);
        }
        [HttpPost("link-provider")]
        public async Task<Results<Ok<JwtResult>, BadRequest, ProblemHttpResult>> LinkAccountWithProvider(Guid userId, string token, CancellationToken cancellationToken = default)
        {
            var userSignResponse = await _signInManagerServices.LinkWithProviderAsync(userId, token, cancellationToken);
            List<Claim> userClaimPayload = MapToJwtPayloadFromUserResponse(userSignResponse);
            JwtResult jwtResult = await _jwtManagement.GenerateTokensAsync(userSignResponse.UserName, userClaimPayload);
            return TypedResults.Ok(jwtResult);
        }
        [HttpPost("logout")]
        public async Task<Results<NoContent, BadRequest, ProblemHttpResult, UnauthorizedHttpResult>> Logout()
        {
            var userName = _userProvider.Username;
            if (userName == null)
            {
                return TypedResults.Unauthorized();
            }
            await _jwtManagement.RemoveRefreshTokenByUserNameAsync(userName);
            return TypedResults.NoContent();
        }
        [HttpPost("refresh-token")]
        public async Task<Results<Ok<JwtResult>, BadRequest, ProblemHttpResult, UnauthorizedHttpResult>> RefreshToken()
        {
            string? accessToken = HttpContext.GetAccessToken();
            string? refreshToken = HttpContext.GetRefreshToken();
            if (string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrWhiteSpace(refreshToken))
            {
                return TypedResults.Unauthorized();
            }
            var jwtResult = await _jwtManagement.RefreshTokenAsync(refreshToken, accessToken);
            return TypedResults.Ok(jwtResult);
        }
        /// <summary>
        ///  Support map from user sign reponse to list claim 
        /// </summary>
        /// <param name="userSign"></param>
        /// <returns>
        ///     Return list claim with type suit for user sign type
        /// </returns>
        private List<Claim> MapToJwtPayloadFromUserResponse(UserSignResponse userSign)
        {
            var claims = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, userSign.UserId.ToString()),
                new(ClaimTypes.Role, userSign.Role.ToString()),
                new(ClaimTypes.GivenName, userSign.UserName)
            };
            if (userSign.FullName is not null)
            {
                claims.Add(new Claim(ClaimTypes.Name, userSign.FullName));
            }
            if (userSign.EmailAddress is not null)
            {
                claims.Add(new Claim(ClaimTypes.Email, userSign.EmailAddress));
            }
            return claims;
        }
        /// <summary>
        ///  Get meta data for link account token
        /// </summary>
        private Dictionary<string, string> GetMetadataForLinkAccountToken(string provider, string identifier)
        {
            var metadataToLinkWithProvider = new Dictionary<string, string>()
                {
                    {"Provider", provider },
                    {"Identifier", identifier}
                };
            return metadataToLinkWithProvider;
        }
    }
}
