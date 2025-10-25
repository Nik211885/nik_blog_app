using Application.Entities;

namespace Application.Services.NotificationTemplateManager.Responses;

public class NotificationTemplateResponse
{
    
}

internal static class NotificationTemplateResponseExtensions
{
    public static NotificationTemplateResponse MapToResponse(this NotificationTemplate notificationTemplate)
    {
        return new NotificationTemplateResponse()
        {

        };
    }   
}