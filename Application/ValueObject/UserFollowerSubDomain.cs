using Application.Entities;

namespace Application.ValueObject;
/// <summary>
///  Defined user follower
/// </summary>
public class UserFollowerSubDomain
{
    /// <summary>
    ///     User id action follower subdomain
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    ///     Domain received  follow 
    /// </summary>
    public Guid FollowerId { get; set; }
    /// <summary>
    ///  Navigation for ef
    /// </summary>
    public User? User { get; set; }
    /// <summary>
    ///  Navigation for ef
    /// </summary>
    public User? Follower { get; set; }
    /// <summary>
    ///     Date follow
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    /// <summary>
    ///     Flag registered receive notification to subdomain
    /// </summary>
    public bool RegisteredReceivedMessage { get; set; }
}