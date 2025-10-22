using Application.Enums;
using Application.ValueObject;

namespace Application.Entities;
/// <summary>
///     Defined model for user
/// </summary>
public class User : AuditEntity
{
    /// <summary>
    ///  Username for user and it unique for each user 
    /// </summary>
    public string UserName { get; set; }
    /// <summary>
    ///  Email for user each user just have one account email
    /// </summary>
    public string? Email { get; set; }
    /// <summary>
    ///  Password for user
    /// </summary>
    public string? Password { get; set; }
    /// <summary>
    ///     First name for user
    /// </summary>
    public string? FirstName { get; set; }
    /// <summary>
    ///     Last name for user
    /// </summary>
    public string? LastName { get; set; }
    /// <summary>
    ///     Full name for user
    /// </summary>
    public string? FullName { get; set; }
    /// <summary>
    ///     Phone number for user and just have one each user
    /// </summary>
    public string? PhoneNumber { get; set; }
    /// <summary>
    ///  Url avatar for user
    /// </summary>
    public string? Avatar { get; set; }
    /// <summary>
    ///  Birthday for user
    /// </summary>
    public DateTimeOffset? BirthDay  { get; set; }
    /// <summary>
    ///  Lock for user
    /// </summary>
    public LockEntity? LockAccount { get; set; }
    /// <summary>
    ///  Bio for user
    /// </summary>
    public string? Bio { get; set; }
    /// <summary>
    ///  Slug for user profile
    /// </summary>
    public string UserCvSlug { get; set; }
    /// <summary>
    ///  subdomain defined for each user
    /// </summary>
    public UserSubDomain UserSubDomain { get; set; }
    /// <summary>
    ///    Navigation to login provider for ef
    /// </summary>
    public ICollection<LoginProvider>? LoginProviders { get; set; }

    /// <summary>
    ///  Defined role for user
    /// </summary>
    public Role Role { get; set; } = Role.User;
    /// <summary>
    ///     Navigation to subject
    /// </summary>
    public ICollection<Subject>? Subjects { get; set; }
}