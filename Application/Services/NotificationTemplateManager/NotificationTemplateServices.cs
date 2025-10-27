using Application.Entities;
using Application.Exceptions;
using Application.Repositories;
using Application.Services.NotificationTemplateManager.Requests;
using Application.Services.NotificationTemplateManager.Responses;
using Microsoft.Extensions.Logging;

namespace Application.Services.NotificationTemplateManager;

public class NotificationTemplateServices
{
    private readonly ILogger<NotificationTemplateServices> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public NotificationTemplateServices(ILogger<NotificationTemplateServices> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    /// <summary>
    ///     Create notification template
    /// </summary>
    /// <param name="request">information for request need to created</param>
    /// <param name="cancellationToken">token need to cancellation action</param>
    /// <returns>
    ///     Return notification response after created template success
    /// </returns>
    public async Task<NotificationTemplateResponse> CreateNotificationTemplateAsync(
        CreateNotificationTemplateRequest request, CancellationToken cancellationToken = default)
    {
        NotificationTemplate? notificationTemplate = await _unitOfWork.NotificationTemplateRepository
            .GetByCodeAsync(request.Code, cancellationToken);
        ThrowHelper.ThrowBusinessErrorWhenExitsItem(notificationTemplate,NotificationTemplateConstMessage.HasExitsCode);
        
        // compare param with params user has passed it
        
        // get all param user has passed 
        notificationTemplate = request.MapToNotificationTemplate(null);
        NotificationTemplateBusinessRule.CreateRule(notificationTemplate)
            .ContentCanNotEmpty()
            .CheckChanelNotificationForTemplateType();

        _unitOfWork.NotificationTemplateRepository.Add(notificationTemplate);
        await _unitOfWork.SaveChangeAsync(cancellationToken);
        return notificationTemplate.MapToResponse();
    }
}