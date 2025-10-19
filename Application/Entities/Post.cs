using Application.Enums;
using Application.ValueObject;

namespace Application.Entities;
/// <summary>
///  Defined model to content it just is post
/// </summary>
public class Post : AuditEntity
{
    /// <summary>
    /// Title for post
    /// </summary>
    public string Title { get; set; }  
    /// <summary>
    /// Content for post
    /// </summary>
    public string Content { get; set; }
    /// <summary>
    ///  Visibility for post
    /// </summary>
    public PostVisibility Visibility { get; set; }
    /// <summary>
    ///  Slug address friendly  for post 
    /// </summary>
    public string PostSlug { get; set; }
    /// <summary>
    /// Cout comment for post
    /// </summary>
    public long CountComments { get; set; }
    /// <summary>
    /// Collection subject to post navigation for ef
    /// </summary>
    public ICollection<PostSubject> PostSubjects { get; set; }
    /// <summary>
    /// Collection reaction to post navigation for ef
    /// </summary>
    public ICollection<ReactionEntity> ReactionPosts { get; set; }
    /// <summary>
    /// Collection comments to post navigation for ef
    /// </summary>
    public ICollection<Comment> Comments { get; set; }
    /// <summary>
    ///  Cout reaction for entity 
    /// </summary>
    public ICollection<CoutReaction> CoutReactions { get; set; }
}