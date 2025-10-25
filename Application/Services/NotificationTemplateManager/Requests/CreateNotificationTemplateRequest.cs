using Application.Entities;

namespace Application.Services.NotificationTemplateManager.Requests;

public class CreateNotificationTemplateRequest
{
    
}

internal static class CreateNotificationTemplateRequestExtension
{
    public static NotificationTemplate MapToNotificationTemplate(this CreateNotificationTemplateRequest request)
    {
        return new NotificationTemplate()
        {

        };
    }
}