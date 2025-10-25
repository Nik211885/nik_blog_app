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

    public UserController(ILogger<UserController> logger, UserManagerServices userManagerServices)
    {
        _logger = logger;
        this._userManagerServices = userManagerServices;
    }
    [HttpPost("create")]
    [ValidationFilter]
    public async Task<Results<Ok<UserResponse>, BadRequest, ProblemHttpResult>> Create([FromBody] CreateUserRequest request, 
        CancellationToken cancellationToken = default)
    {
        UserResponse userResponse = await _userManagerServices.CreateUser(request, cancellationToken);
        return TypedResults.Ok(userResponse);
    }
}
