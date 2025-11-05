using System.Text.Json;
using Application.Adapters;
using Application.Entities;
using Application.Exceptions;
using Application.Repositories;
using Application.Services.Shared;
using Application.Services.SignInManager.Models;
using Application.Services.SignInManager.Requests;
using Application.Services.SignInManager.Responses;
using Application.Services.UserManager;
using Microsoft.Extensions.Logging;

namespace Application.Services.SignInManager;

public class SignInManagerServices
{
    private readonly ILogger<SignInManagerServices> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SignInConfig _signInConfig;
    private readonly ICache _cache;
    private readonly ITokenEncryptionService _tokenEncryptionServices;
    public SignInManagerServices(ILogger<SignInManagerServices> logger, IUnitOfWork unitOfWork,
    SignInConfig signInConfig, ITokenEncryptionService tokenEncryptionService, ICache cache)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _signInConfig = signInConfig;
        _cache = cache;
        _tokenEncryptionServices = tokenEncryptionService;
    }
    /// <summary>
    ///     Login user with user name and passowrd
    /// </summary>
    /// <param name="request">include user name and password</param>
    /// <param name="cancellationToken">token to cancelaltion login</param>
    /// <returns>
    ///     Return user sign response when login success with user name and password
    /// </returns>
    public async Task<UserSignResponse> PasswordLoginAsync(PasswordLoginRequest request, CancellationToken cancellationToken = default)
    {
        User? user = await _unitOfWork.UserRepository.GetByUsernameAsync(request.UserName, cancellationToken);
        ThrowHelper.ThrowWhenNotFoundItem(user, SignInConstMessage.LoginPasswordInval);
        var userManagerRule = UserManagerBusinessRule.CreateRule(user);
        if (!user.EmailConfirmed || !userManagerRule.CheckPasswordBoolean(request.Passsword))
        {
            await AccessFailedAsync(user, cancellationToken);
            ThrowHelper.ThrowWhenNotFoundItem(user, SignInConstMessage.LoginPasswordInval);
        }
        if (userManagerRule.CheckLockAccountBoolean())
        {
            await ResetLockCountAndTimeAccountAsync(user, cancellationToken);
        }
        return user.MapToSignResponse();
    }
    /// <summary>
    ///     Link provider for user 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token">token to validation action link with provider</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns>
    ///     Return usersign response when link account to provider success
    /// </returns>
    public async Task<UserSignResponse> LinkWithProviderAsync(LinkWithProviderRequest request, string token, CancellationToken cancellationToken = default)
    {
        User? user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId, cancellationToken);
        ThrowHelper.ThrowWhenNotFoundItem(user, SignInConstMessage.UserNotFound);
        var userManagerRule = UserManagerBusinessRule.CreateRule(user);
        if (userManagerRule.CheckLockAccountBoolean())
        {
            ThrowHelper.ThrowWhenBusinessError(string.Format(SignInConstMessage.AccountHasLock, user.LockAccount?.ReasonLock, user.LockAccount?.LockToTime));
        }
        if (ValidSignToken(user, token, SignInTokenType.LinkProvider))
        {
            ThrowHelper.ThrowWhenBusinessError(SignInConstMessage.HasErrorWhenLinkWithProvider);
        }
        LoginProvider? loginWithProvider = await _unitOfWork.LoginProviderRepository.GetByProviderAsync(request.Provider, request.Identifier, cancellationToken);
        ThrowHelper.ThrowBusinessErrorWhenExitsItem(loginWithProvider, SignInConstMessage.LoginProviderHasExits);
        loginWithProvider = request.MapToProvider();
        _unitOfWork.LoginProviderRepository.Add(loginWithProvider);
        await _unitOfWork.SaveChangeAsync(cancellationToken);
        return user.MapToSignResponse();
    }
    /// <summary>
    ///     Valida authozization token when user must get token and user lock account
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="token"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     Return information user when exchange token and account don't lock 
    /// </returns>
    public async Task<UserSignResponse> ValidExchangeTokenAsync(Guid userId, string token, CancellationToken cancellationToken = default)
    {
        User? userById = await _unitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);
        ThrowHelper.ThrowWhenNotFoundItem(userById, SignInConstMessage.UserNotFound);
        var userManagerRule = UserManagerBusinessRule.CreateRule(userById);
        if (userManagerRule.CheckLockAccountBoolean())
        {
            ThrowHelper.ThrowWhenBusinessError(string.Format(SignInConstMessage.AccountHasLock, userById.LockAccount?.ReasonLock, userById.LockAccount?.LockToTime));
        }
        var keyAuthozizationCode = $"authozization-code:user-id{userId}:token{token}";
        //  if authozization has use exchange token before rejected it
        if (await _cache.TryGetItem(keyAuthozizationCode, out string? _))
        {
            ThrowHelper.ThrowWhenBusinessError(SignInConstMessage.TokenExchangeExpired);
        }
        if (ValidSignToken(userById, token, SignInTokenType.AuthorizationCode))
        {
            ThrowHelper.ThrowWhenBusinessError(SharedConstMessage.Error);
        }
        await _cache.SetItem(keyAuthozizationCode, "_", TimeSpan.FromMinutes(1));
        return userById.MapToSignResponse();
    }
    /// <summary>
    ///  Generator token with sign manager
    /// </summary>
    /// <param name="user">information sign encrypt</param>
    /// <param name="tokenType">token type is action with verify</param>
    /// <returns>
    ///     Return token with base 64 encrypt data about sign token services
    /// </returns>
    public string GeneratorSignToken(User user, SignInTokenType tokenType, int minuteExpriedToken = 1)
    {
        var userPayload = new SignInTokenPayload()
        {
            UserId = user.Id,
            SignInTokenType = tokenType,
            TokenExpired = DateTimeOffset.UtcNow.AddMinutes(minuteExpriedToken)
        };
        string jsonUserPayload = JsonSerializer.Serialize(userPayload);
        string token = _tokenEncryptionServices.Encrypt(jsonUserPayload);
        return token;
    }
    /// <summary>
    ///     Verify token is compare all value in payload with information match user 
    /// </summary>
    /// <param name="token">token need to check</param>
    /// <returns>
    ///     Return SignInTokenPayload
    /// </returns>
    private SignInTokenPayload DecodeSignToken(string token)
    {
        string jsonTokenPayload = _tokenEncryptionServices.Decrypt(token);
        SignInTokenPayload? signInTokenPayload = JsonSerializer.Deserialize<SignInTokenPayload>(jsonTokenPayload);
        ThrowHelper.ThrowWhenBusinessError(UserManageMessageConst.InvalidUserToken);
        return signInTokenPayload;
    }
    /// <summary>
    ///  Valid sign token
    /// </summary>
    /// <param name="user">user information</param>
    /// <param name="token">token need check</param>
    /// <param name="tokenType">token type</param>
    /// <returns>
    ///     Return true if invalid otherwise false
    /// </returns>
    public bool ValidSignToken(User user, string token, SignInTokenType tokenType)
    {
        var signTokenPayload = DecodeSignToken(token);
        return signTokenPayload.TokenExpired < DateTime.UtcNow
            || signTokenPayload.UserId != user.Id
            || signTokenPayload.SignInTokenType != tokenType;
    }
    /// <summary>
    ///     Condition when user login fail 
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    private async ValueTask AccessFailedAsync(User user, CancellationToken cancellationToken = default)
    {
        // You can specific information to lock account or minute when login fail with action to di container
        user.CoutLoginFail += 1;
        if (user.CoutLoginFail >= _signInConfig.LoginFailToLockAccount)
        {
            user.LockAccount ??= new();
            user.LockAccount.IsLock = true;
            user.LockAccount.LockToTime = DateTimeOffset.UtcNow.AddMinutes(_signInConfig.MinuteLockAccountWhenLoginFail);
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }
    }
    /// <summary>
    ///  Reset lock count and time for lock entity in account
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private async ValueTask ResetLockCountAndTimeAccountAsync(User user, CancellationToken cancellationToken = default)
    {
        if (user.LockAccount is null)
        {
            throw new Exception("Tài khoản không bị khóa");
        }
        if (user.LockAccount!.LockToTime >= DateTimeOffset.UtcNow)
        {
            ThrowHelper.ThrowWhenBusinessError(string.Format(SignInConstMessage.AccountHasLock, user.LockAccount!.ReasonLock, user.LockAccount!.LockToTime?.ToString("dd/MM/yyyy")));
        }
        else
        {
            user.CoutLoginFail = 0;
            user.LockAccount.IsLock = false;
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }
    }
}
