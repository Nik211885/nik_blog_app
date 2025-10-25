namespace Application.Entities;
/// <summary>
///  For information for login provider with sso 
/// </summary>
public class LoginProvider : BaseEntity
{
    /// <summary>
    ///     Name for provider support sso
    /// </summary>
    public string Provider { get; set; } = null!;

    /// <summary>
    ///     Identifier for user in provider
    /// </summary>
    public string Identifier { get; set; } = null!;
    /// <summary>
    ///     Navigation to user each user has multiple provider 
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    ///     Navigation for ef
    /// </summary>
    public User? User { get; set; }
}