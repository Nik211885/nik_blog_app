using Application.Enums;

namespace Application.Entities;

/// <summary>
///  Template for notification
/// </summary>
public class NotificationTemplate : BaseEntity
{
    /// <summary>
    ///  Code for notification template
    /// </summary>
    public string Code { get; set; } =  String.Empty;
    /// <summary>
    ///  Subject for notification
    /// </summary>
    public string Subject { get; set; }  = String.Empty;
    /// <summary>
    ///  Content is html in template
    /// </summary>
    public string? ContentHtml { get; set; }
    /// <summary>
    ///   Content text for notification
    /// </summary>
    public string? ContentText { get; set; }

    /// <summary>
    ///  Flags active for notification template
    /// </summary>
    public bool IsActive { get; set; } = true;
    /// <summary>
    ///   Chanel for notification will push
    /// </summary>
    public NotificationChanel NotificationChanel { get; set; }
    /// <summary>
    ///  Define  notification services type for template
    /// </summary>
    public NotificationServicesType NotificationServicesType { get; set; }
    
}