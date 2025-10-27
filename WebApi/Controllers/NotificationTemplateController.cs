using Application.Services.NotificationTemplateManager;
using Application.Services.NotificationTemplateManager.Requests;
using Application.Services.NotificationTemplateManager.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApi.Pipelines.Filters;

namespace WebApi.Controllers;
[ApiController]
[Route("api/notification-templates")]
public class NotificationTemplateController
{
    private readonly ILogger<NotificationTemplateController> _logger;
    private readonly NotificationTemplateServices _notificationTemplateServices;

    public NotificationTemplateController(ILogger<NotificationTemplateController> logger, NotificationTemplateServices notificationTemplateServices)
    {
        _logger = logger;
        _notificationTemplateServices = notificationTemplateServices;
    }
    [HttpPost("create")]
    [ValidationFilter]
    public async Task<Results<Ok<NotificationTemplateResponse>, BadRequest, ProblemHttpResult, UnauthorizedHttpResult>>
        CreateMailInfo(CreateNotificationTemplateRequest request, CancellationToken cancellationToken = default)
    {
        NotificationTemplateResponse notificationTemplateResponse = await _notificationTemplateServices
            .CreateNotificationTemplateAsync(request, cancellationToken);
        return TypedResults.Ok(notificationTemplateResponse);
    }
    
}