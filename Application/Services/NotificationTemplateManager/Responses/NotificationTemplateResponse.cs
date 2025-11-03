using Application.Entities;
using Application.Enums;
using Application.Services.ArgumentManager.Responses;
using Application.Services.MailInfoManager.Responses;

namespace Application.Services.NotificationTemplateManager.Responses;

public  class NotificationTemplateResponse
{
    public Guid Id { get; init; }
    public string Code { get; init; } =  string.Empty;
    public string Subject { get; init; } = string.Empty;
    public string? ContentHtml { get; init; }
    public string? ContentText { get; init; }
    public bool IsActive { get; init; }
    public NotificationChanel? NotificationChanel { get; init; }
    public NotificationServicesType? NotificationServicesType { get; init; }
    public MailInfoResponse? MailInfo { get; init; }
    public ICollection<ArgumentResponse>? Arguments { get; init; }
}

public static class NotificationTemplateResponseExtensions
{
    public static NotificationTemplateResponse MapToResponse(this NotificationTemplate notificationTemplate)
    {
        return new NotificationTemplateResponse()
        {
            Id = notificationTemplate.Id,
            Code = notificationTemplate.Code,
            Subject = notificationTemplate.Subject,
            ContentHtml = notificationTemplate.ContentHtml,
            ContentText = notificationTemplate.ContentText,
            IsActive = notificationTemplate.IsActive,
            NotificationChanel = notificationTemplate.NotificationChanel,
            NotificationServicesType = notificationTemplate.NotificationServicesType,
            MailInfo = notificationTemplate.MailInfo?.MapToResponse(),
            Arguments = notificationTemplate.Arguments?.Select(x=>x.MapToResponse()).ToList()
        };
    }   
}