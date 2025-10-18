using Application.Entities;
using Application.Enums;

namespace Application.ValueObject;

public abstract class ReactionEntity<TEntity>
{
    /// <summary>
    ///  User has reaction post
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    /// Post to get reaction
    /// </summary>
    public Guid EntityId { get; set; }
    /// <summary>
    ///     Navigation to user for ef
    /// </summary>
    public User User { get; set; }
    /// <summary>
    ///     Navigation to post for ef
    /// </summary>
    public TEntity Entity { get; set; }
    /// <summary>
    ///  Date has reaction with post
    /// </summary>
    public DateTimeOffset ReactionAt {get;} = DateTimeOffset.UtcNow;
    /// <summary>
    /// Reaction type for post
    /// </summary>
    public ReactionType ReactionType { get; set; }
}
/// <summary>
/// Reaction for comment
/// </summary>
public class CommentReaction : ReactionEntity<Comment>;
/// <summary>
/// Reaction for post
/// </summary>
public class PostReaction : ReactionEntity<Post>;