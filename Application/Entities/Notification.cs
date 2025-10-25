namespace Application.Entities;

/// <summary>
///  Defined notification for chanel is push application
/// </summary>
public class Notification : BaseEntity
{
    /// <summary>
    ///     Subject for notification
    /// </summary>
    public string Subject { get; set; }
    /// <summary>
    ///  Content for notification
    /// </summary>
    public string Content { get; set; }
    /// <summary>
    ///  User has received notification
    /// </summary>
    public Guid UserReceivedId { get; set; }
    /// <summary>
    ///  Navigation for user in ef
    /// </summary>
    public User UserReceived { get; set; }  
    /// <summary>
    ///  Time has sent notification
    /// </summary>
    public DateTimeOffset  SendTimeAt { get; set; }
    /// <summary>
    /// Open pages when user click notification
    /// /// </summary>
    public string? UrlNavigation { get; set; }
    /// <summary>
    ///  User has sent notification
    /// </summary>
    public Guid UserSendById { get; set; }
    /// <summary>
    ///  Navigation user sent by for ef
    /// </summary>
    public User UserSendBy { get; set; }

    /// <summary>
    ///  Flags check user received has read message
    /// </summary>
    public bool IsRead { get; set; } = false;
}