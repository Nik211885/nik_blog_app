using Application.ValueObject;

namespace Application.Entities;

/// <summary>
///  Information about mail will send for with each services
/// </summary>
public class MailInfo : BaseEntity
{
    /// <summary>
    /// Host specific for mail
    /// </summary>
    public string Host { get; set; } = string.Empty;
    /// <summary>
    ///  Port for mail services
    /// </summary>
    public int Port { get; set; }
    /// <summary>
    ///     Name for enterprise
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    ///     Email address
    /// </summary>
    public string EmailId { get; set; } = string.Empty;
    /// <summary>
    ///     Username for email address
    /// </summary>
    public string UserName { get; set; } = string.Empty;
    /// <summary>
    ///     Application password
    /// </summary>
    public string Password { get; set; } = string.Empty;
    /// <summary>
    ///     Enable ssl for mail services
    /// </summary>
    public bool EnableSsl { get; set; }

    /// <summary>
    ///  Flags active for mail config
    /// </summary>
    public bool IsActive { get; set; } = true;
    /// <summary>
    ///  Navigation for template
    /// </summary>
    public ICollection<NotificationTemplate>? NotificationTemplates { get; set; }
}