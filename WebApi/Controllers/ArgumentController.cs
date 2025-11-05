using Application.Services.ArgumentManager;
using Application.Services.ArgumentManager.Requests;
using Application.Services.ArgumentManager.Responses;
using Application.Services.NotificationTemplateManager.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApi.Pipelines.Filters;

namespace WebApi.Controllers;
[ApiController]
[Route("api/arguments")]
public class ArgumentController : ControllerBase
{
    private readonly ILogger<ArgumentController> _logger;
    private readonly ArgumentServices _argumentServices;

    public ArgumentController(ILogger<ArgumentController> logger, ArgumentServices argumentServices)
    {
        _logger = logger;
        _argumentServices = argumentServices;
    }
    [HttpPost("create")]
    [ValidationFilter]
    public async Task<Results<Ok<ArgumentResponse>, BadRequest, ProblemHttpResult, UnauthorizedHttpResult>>
        CreateArgument(CreateArgumentRequest request, CancellationToken cancellationToken = default)
    {
        var argument = await _argumentServices.CreateArgumentAsync(request, cancellationToken);
        return TypedResults.Ok(argument.MapToResponse());
    }

    [HttpPut("update")]
    [ValidationFilter]
    public async Task<Results<Ok<ArgumentResponse>, BadRequest, ProblemHttpResult, UnauthorizedHttpResult>>
        UpdateArgument(UpdateArgumentResponse request, CancellationToken cancellationToken = default)
    {
        var argument = await _argumentServices.UpdateArgumentAsync(request, cancellationToken);
        return TypedResults.Ok(argument.MapToResponse());
    }
}