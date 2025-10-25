using Application.ValueObject;

namespace Application.Entities;
/// <summary>
///     Defined model comment in application
/// </summary>
public class Comment : AuditEntity
{
    /// <summary>
    ///  Content for comment
    /// </summary>
    public string ContentComment { get; set; } =  string.Empty;
    /// <summary>
    /// Post to get comment
    /// </summary>
    public Guid PostId { get; set; }
    /// <summary>
    /// Navigation post for ef
    /// </summary>
    public Post? Post { get; set; }
    /// <summary>
    ///  Parent id  comment
    /// </summary>
    public Guid CommentParentId { get; set; }
    /// <summary>
    ///  Reaction for comment
    /// </summary>
    public ICollection<ReactionEntity>? ReactionComments { get; set; }
    /// <summary>
    ///  Cout reaction for comment
    /// </summary>
    public ICollection<CoutReaction>? CoutReactions { get; set; }
    /// <summary>
    ///  Comment parent
    /// </summary>
    public Comment? CommentParent { get; set; }
    /// <summary>
    ///  Collection with comment child
    /// </summary>
    public ICollection<Comment>? CommentChilds { get; set; }  
}