using Application.Adapters;
using Application.Services.UserManager;
using Application.Services.UserManager.Requests;
using Application.Services.UserManager.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApi.Pipelines.Filters;

namespace WebApi.Controllers;
[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly UserManagerServices _userManagerServices;
    private readonly IUserProvider _userProvider;

    public UserController(ILogger<UserController> logger, UserManagerServices userManagerServices, IUserProvider userProvider)
    {
        _logger = logger;
        _userManagerServices = userManagerServices;
        _userProvider = userProvider;
    }
    [HttpPost("create")]
    [ValidationFilter]
    public async Task<Results<Ok<UserResponse>, BadRequest, ProblemHttpResult>> CreateUser([FromBody] CreateUserRequest request, 
        CancellationToken cancellationToken = default)
    {
        UserResponse userResponse = await _userManagerServices.CreateUserAsync(request, cancellationToken);
        return TypedResults.Ok(userResponse);
    }

    [HttpPut("update")]
    public async Task<Results<Ok<UserResponse>, BadRequest, UnauthorizedHttpResult, ProblemHttpResult>> UpdateUser(UpdateUserRequest request,
        CancellationToken cancellationToken = default)
    {
        UserResponse userResponse = await _userManagerServices.UpdateUserAsync(_userProvider.UserId,request);
        return TypedResults.Ok(userResponse);
    }

    [HttpPost("lock")]
    public async Task<Results<Ok<UserResponse>, BadRequest, UnauthorizedHttpResult, ProblemHttpResult>> LockAccount(
        LockAccountRequest request, CancellationToken cancellationToken = default)
    {
        UserResponse userResponse = await _userManagerServices.LockAccountAsync(request, cancellationToken);
        return TypedResults.Ok(userResponse);
    }
    [HttpPost("un-lock")]
    public async Task<Results<Ok<UserResponse>, BadRequest, UnauthorizedHttpResult, ProblemHttpResult>> UnlockAccount(
        Guid userId, CancellationToken cancellationToken = default)
    {
        UserResponse userResponse = await _userManagerServices.UnLockAccountAsync(userId, cancellationToken);
        return TypedResults.Ok(userResponse);
    }
}
