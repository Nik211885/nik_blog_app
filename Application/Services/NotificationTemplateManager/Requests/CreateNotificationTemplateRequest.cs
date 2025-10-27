using System.ComponentModel.DataAnnotations;
using Application.Entities;
using Application.Enums;

namespace Application.Services.NotificationTemplateManager.Requests;

public class CreateNotificationTemplateRequest
{
    [Required(ErrorMessage = NotificationTemplateConstMessage.CodeIsRequired)]
    public string Code { get; init; } = string.Empty;
    [Required(ErrorMessage = NotificationTemplateConstMessage.SubjectIsRequired)]
    public string Subject { get; init; } = string.Empty;
    public string? ContentHtml { get; init; }
    public string? ContentText { get; init; }
    public NotificationChanel NotificationChanel { get; init; }
    public NotificationServicesType NotificationServicesType { get; init; }
    public Guid? MailInfoId { get; init; }
    public ICollection<Guid>? ArgumentsId { get; init; }
}

internal static class CreateNotificationTemplateRequestExtension
{
    public static NotificationTemplate MapToNotificationTemplate(this CreateNotificationTemplateRequest request,
        ICollection<Arguments>? arguments)
    {
        return new NotificationTemplate()
        {
            Code = request.Code,
            Subject = request.Subject,
            ContentHtml = request.ContentHtml,
            ContentText = request.ContentText,
            NotificationChanel = request.NotificationChanel,
            NotificationServicesType = request.NotificationServicesType,
            MailInfoId = request.MailInfoId,
            Arguments = arguments
        };
    }
}