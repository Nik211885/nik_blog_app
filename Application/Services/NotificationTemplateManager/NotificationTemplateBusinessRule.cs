using Application.Entities;
using Application.Enums;
using Application.Exceptions;
using Application.Extensions;

namespace Application.Services.NotificationTemplateManager;
/// <summary>
///  Defined business notification template
/// </summary>
internal class NotificationTemplateBusinessRule
{
    private readonly NotificationTemplate _notificationTemplate;

    private NotificationTemplateBusinessRule(NotificationTemplate notificationTemplate)
    {
        _notificationTemplate = notificationTemplate;
    }
    /// <summary>
    ///     Check chanel notification for template type
    ///     is just chanel mail in template just has added mail info
    ///     and with chanel mail must add mail info
    /// </summary>
    public NotificationTemplateBusinessRule CheckChanelNotificationForTemplateType()
    {
        if (_notificationTemplate.NotificationChanel is NotificationChanel.SendEmail or NotificationChanel.All
            && _notificationTemplate.MailInfoId is null)
        {
            ThrowHelper.ThrowWhenBusinessError(NotificationTemplateConstMessage.ChanelMailNeedAddEmailInfo);
        }

        if (_notificationTemplate.NotificationChanel is not NotificationChanel.SendEmail and NotificationChanel.All
            && _notificationTemplate.MailInfo is not null)
        {
            ThrowHelper.ThrowWhenBusinessError(NotificationTemplateConstMessage.NotChanelMailNotNeedAddEmailInfo);
        }

        return this;
    }
    /// <summary>
    ///     Template content can not be null in text content and html content
    /// </summary>
    public NotificationTemplateBusinessRule ContentCanNotEmpty()
    {
        if (string.IsNullOrWhiteSpace(_notificationTemplate.ContentHtml) 
            && string.IsNullOrWhiteSpace(_notificationTemplate.ContentText))
        {
            ThrowHelper.ThrowWhenBusinessError(NotificationTemplateConstMessage.ContentCanNotBeNull);
        }

        return this;
    }
    /// <summary>
    ///     Compare argument in content arguments must equals argument interpolate in template
    /// </summary>
    public NotificationTemplateBusinessRule CompareArgumentInContent()
    {
        string content = _notificationTemplate.ContentHtml ?? _notificationTemplate.ContentText
            ?? throw new ArgumentNullException(nameof(content));
        var argumentsInterpolate = content.GetAllArgumentsInterpolateInTemplate().ToList();
        if (argumentsInterpolate.Count != _notificationTemplate.Arguments?.Count)
        {
            ThrowHelper.ThrowWhenBusinessError(NotificationTemplateConstMessage.ArgumentInContentMustEqualsArguments);
        }
        return this;
    }
    /// <summary>
    ///    Check mail infor for template push mail when push to email must exits mail infor and it support for mail chanel
    /// </summary>
    /// <returns></returns>
    public NotificationTemplateBusinessRule CheckMailInfoForTemplatePushMail()
    {
        if (_notificationTemplate.MailInfoId is null
            || (_notificationTemplate.NotificationChanel is not NotificationChanel.SendEmail or NotificationChanel.All))
        {
            ThrowHelper.ThrowWhenBusinessError(NotificationTemplateConstMessage.CanNotFindMailInfoToSendTemplate);
        }
        return this;
    }
    /// <summary>
    ///  Create new instance for notification business 
    /// </summary>
    /// <param name="notificationTemplate"></param>
    /// <returns></returns>
    public static NotificationTemplateBusinessRule CreateRule(NotificationTemplate notificationTemplate)
    {
        return new NotificationTemplateBusinessRule(notificationTemplate);
    }
}