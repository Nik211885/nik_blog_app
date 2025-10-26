using Application.Entities;
using Application.Enums;
using Application.Exceptions;

namespace Application.Services.MailInfoManager;
/// <summary>
///  Defined for all notification business rule 
/// </summary>
internal static class MailInfoBusinessRule
{
    /// <summary>
    ///     Rule make sure mail info just has added to template with notification to chanel mail
    /// </summary>
    /// <param name="notificationTemplate">notfication template</param>
    public static void MailChanelMustAddToTemplate(NotificationTemplate notificationTemplate)
    {
        if (notificationTemplate.NotificationChanel is not (NotificationChanel.All or NotificationChanel.SendEmail))
        {
            ThrowHelper.ThrowWhenBusinessError(MailInfoConstMessage.NotificationNotUseMailChanel);
        }
    }
}