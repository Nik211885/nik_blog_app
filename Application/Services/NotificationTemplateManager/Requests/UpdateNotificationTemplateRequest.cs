using System.ComponentModel.DataAnnotations;
using Application.Entities;
using Application.Enums;

namespace Application.Services.NotificationTemplateManager.Requests;

public class UpdateNotificationTemplateRequest
{
    public Guid NotificationTemplateId { get; init; }
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
    public bool IsActive { get; init; }
}

public static class UpdateNotificationTemplateRequestExtensions
{
    public static void MapToNotificationTemplate(this UpdateNotificationTemplateRequest request,
     NotificationTemplate notificationTemplate, ICollection<Arguments>? arguments)
    {
        notificationTemplate.Code = request.Code;
        notificationTemplate.Subject = request.Subject;
        notificationTemplate.ContentHtml = request.ContentHtml;
        notificationTemplate.ContentText = request.ContentText;
        notificationTemplate.NotificationChanel = request.NotificationChanel;
        notificationTemplate.NotificationServicesType = request.NotificationServicesType;
        notificationTemplate.MailInfoId = request.MailInfoId;
        notificationTemplate.Arguments = arguments;
        notificationTemplate.IsActive = request.IsActive;
    }
}
