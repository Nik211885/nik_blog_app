namespace Application.Entities;
/// <summary>
///  For information for login provider with sso 
/// </summary>
public class LoginProvider : BaseEntity<Guid>
{
    /// <summary>
    ///     Name for provider support sso
    /// </summary>
    public string Provider { get; set; }
    /// <summary>
    ///     Identifier for user in provider
    /// </summary>
    public string Identifier { get; set; }
    /// <summary>
    ///     Navigation to user each user has multiple provider 
    /// </summary>
    public string UserId { get; set; }
    /// <summary>
    ///     Navigation for ef
    /// </summary>
    public User User { get; set; }
}