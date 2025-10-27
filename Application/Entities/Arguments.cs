namespace Application.Entities;

/// <summary>
///  Defined argument data for notification template
/// </summary>
public class Arguments : BaseEntity
{
    /// <summary>
    ///  Code for data after query
    /// </summary>
    public string Code { get; init; } = string.Empty;
    /// <summary>
    ///    Description for argument
    /// </summary>
    public string Description { get; init; } = string.Empty;
    /// <summary>
    ///  Query to get data for template
    /// </summary>
    public string Query { get; set; } = string.Empty;
    /// <summary>
    ///  Navigation for ef
    /// </summary>
    public ICollection<NotificationTemplate>? NotificationTemplates { get; init; }
}