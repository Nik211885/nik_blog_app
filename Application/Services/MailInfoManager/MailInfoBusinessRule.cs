using Application.Entities;
using Application.Enums;
using Application.Exceptions;

namespace Application.Services.MailInfoManager;
/// <summary>
///  Defined for all notification business rule 
/// </summary>
internal class MailInfoBusinessRule
{
    private readonly NotificationTemplate _notificationTemplate;

    private MailInfoBusinessRule(NotificationTemplate notificationTemplate)
    {
        _notificationTemplate = notificationTemplate;
    }
    /// <summary>
    ///     Rule make sure mail info just has added to template with notification to chanel mail
    /// </summary>
    public MailInfoBusinessRule MailChanelMustAddToTemplate()
    {
        if (_notificationTemplate.NotificationChanel is not (NotificationChanel.All or NotificationChanel.SendEmail))
        {
            ThrowHelper.ThrowWhenBusinessError(MailInfoConstMessage.NotificationNotUseMailChanel);
        }

        return this;
    }
    /// <summary>
    ///  Crate instance for mail business rule
    /// </summary>
    /// <param name="notificationTemplate"></param>
    /// <returns></returns>
    public static MailInfoBusinessRule CreateRule(NotificationTemplate notificationTemplate)
    {
        return new MailInfoBusinessRule(notificationTemplate);
    }
}