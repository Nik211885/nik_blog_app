using Application.Adapters;
using Application.Entities;
using Application.Enums;
using Application.Services.NotificationTemplateManager;
using Application.Services.UserManager;
using Application.Services.UserManager.Models;
using Application.Services.UserManager.Requests;
using Application.Services.UserManager.Responses;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WebApi.Pipelines.Filters;
using WebApi.Services.BackgroundTaskQueue;

namespace WebApi.Controllers;
[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly UserManagerServices _userManagerServices;
    private readonly IUserProvider _userProvider;
    private readonly NotificationTemplateServices _notificationTemplateServices;
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IBackgroundTaskQueue _taskQueue;

    public UserController(ILogger<UserController> logger, UserManagerServices userManagerServices, IUserProvider userProvider,
    NotificationTemplateServices notificationTemplateServices, IBackgroundTaskQueue taskQueue,
    ApplicationDbContext applicationDbContext)
    {
        _logger = logger;
        _userManagerServices = userManagerServices;
        _userProvider = userProvider;
        _notificationTemplateServices = notificationTemplateServices;
        _taskQueue = taskQueue;
        _applicationDbContext = applicationDbContext;
    }
    [HttpPost("create")]
    [ValidationFilter]
    public async Task<Results<Ok<UserResponse>, BadRequest, ProblemHttpResult>> CreateUser([FromBody] CreateUserRequest request, 
        CancellationToken cancellationToken = default)
    {
        var user = await _userManagerServices.CreateUserAsync(request, cancellationToken);
        if (!string.IsNullOrWhiteSpace(user.Email))
        {
            _taskQueue.QueueBackgoundWorkItem(async token =>
            {
                 await _notificationTemplateServices.SendMailWithTemplateServiceAsync(
                    NotificationServicesType.CreateNewAccount,
                    user.Email,
                    user.FullName, token,
                    defaultParamsInMessage: new Dictionary<string, object>
                    {
                        {"link", _userManagerServices.GeneratorUserToken(user , UserTokenType.CreateAccount)}
                    });
            });
        }
        return TypedResults.Ok(user.MapToResponse());
    }

    [HttpPut("update")]
    public async Task<Results<Ok<UserResponse>, BadRequest, UnauthorizedHttpResult, ProblemHttpResult>> UpdateUser(UpdateUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManagerServices.UpdateUserAsync(_userProvider.UserId, request, cancellationToken);
        return TypedResults.Ok(user.MapToResponse());
    }

    [HttpPost("lock")]
    public async Task<Results<Ok<UserResponse>, BadRequest, UnauthorizedHttpResult, ProblemHttpResult>> LockAccount(
        LockAccountRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManagerServices.LockAccountAsync(request, cancellationToken);
        return TypedResults.Ok(user.MapToResponse());
    }
    [HttpPost("un-lock")]
    public async Task<Results<Ok<UserResponse>, BadRequest, UnauthorizedHttpResult, ProblemHttpResult>> UnlockAccount(
        Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManagerServices.UnLockAccountAsync(userId, cancellationToken);
        return TypedResults.Ok(user.MapToResponse());
    }
    [HttpPost("confirm-email")]
    public async Task<Results<NoContent, BadRequest, ProblemHttpResult>> ConfirmEmail(
        Guid userId, string token, CancellationToken cancellationToken = default)
    {
        await _userManagerServices.ConfirmEmailAsync(userId, token, cancellationToken);

        return TypedResults.NoContent();
    }
    [HttpPost("reset-password")]
    public async Task<Results<NoContent, BadRequest, ProblemHttpResult>> ResetPassword(
        Guid userId, string token, ResetPasswordRequest request, CancellationToken cancellationToken = default)
    {
        await _userManagerServices.ResetPasswordAsync(userId, token, request, cancellationToken);
        return TypedResults.NoContent();
    }
    [HttpPost(("send-mail/services-type"))]
    public async Task<Results<NoContent, BadRequest<string>, ProblemHttpResult>> SendMailForServicesType(UserTokenType userTokenType, string email, CancellationToken cancellationToken = default)
    {
        var user = await _applicationDbContext.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        if (user is null)
        {
            return TypedResults.BadRequest("Không tìm thấy tài khoản có địa chỉ email này");
        }
        if (user.LockAccount?.IsLock ?? false)
        {
            return TypedResults.BadRequest("Tài khoản đang bị khóa");
        }
        _taskQueue.QueueBackgoundWorkItem(async token =>
        {
            var notificaitonType = userTokenType switch
            {
                UserTokenType.CreateAccount => NotificationServicesType.CreateNewAccount,
                UserTokenType.ConfirmEmail => NotificationServicesType.ConfirmEmail,
                UserTokenType.ResetPassword => NotificationServicesType.ResetPassword,
                _=> throw new NotImplementedException()
            };
            await _notificationTemplateServices
            .SendMailWithTemplateServiceAsync(notificaitonType, email, user.FullName, token,
            defaultParamsInMessage: new Dictionary<string, object>
            {
                {"link", _userManagerServices.GeneratorUserToken(user, userTokenType)}   
            });
        });
        return TypedResults.NoContent();
    }
}
