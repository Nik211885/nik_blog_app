using Application.Entities;
using Application.Enums;

namespace Application.ValueObject;
public class ReactionEntity
{
    /// <summary>
    ///  User has reaction post
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    /// entity to get reaction
    /// </summary>
    public Guid EntityId { get; set; }
    /// <summary>
    ///  Type for reaction
    /// </summary>
    public ReactionEntityType EntityType { get; set; }
    /// <summary>
    ///     Navigation to user for ef
    /// </summary>
    public User? User { get; set; }
    /// <summary>
    ///  Date has reaction with post
    /// </summary>
    public DateTimeOffset ReactionAt {get;} = DateTimeOffset.UtcNow;
    /// <summary>
    /// Reaction type for post
    /// </summary>
    public ReactionType ReactionType { get; set; }
}