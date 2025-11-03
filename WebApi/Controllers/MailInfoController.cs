using Application.Services.MailInfoManager;
using Application.Services.MailInfoManager.Requests;
using Application.Services.MailInfoManager.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApi.Pipelines.Filters;

namespace WebApi.Controllers;

[ApiController]
[Route("api/mail-info")]
public class MailInfoController : ControllerBase
{
    private readonly ILogger<MailInfoController> _logger;
    private readonly MailInfoServices _mailInfoServices;

    public MailInfoController(ILogger<MailInfoController> logger, MailInfoServices mailInfoServices)
    {
        _logger = logger;
        _mailInfoServices = mailInfoServices;
    }

    [HttpPost("create")]
    [ValidationFilter]
    public async Task<Results<Ok<MailInfoResponse>, BadRequest, ProblemHttpResult, UnauthorizedHttpResult>>
        CreateMailInfo(CreateMailInfoRequest request, CancellationToken cancellationToken = default)
    {
        var mailInfo = await _mailInfoServices.CreateMailInfoAsync(request, cancellationToken);
        return TypedResults.Ok(mailInfo.MapToResponse());
    }

    [HttpPut("update")]
    [ValidationFilter]
    public async Task<Results<Ok<MailInfoResponse>, BadRequest, ProblemHttpResult, UnauthorizedHttpResult>>
        UpdateMailInfo(UpdateMailInfoRequest request, CancellationToken cancellationToken = default)
    {
        var mailInfo = await _mailInfoServices.UpdateMailInfoAsync(request, cancellationToken);
        return TypedResults.Ok(mailInfo.MapToResponse());
    }
}