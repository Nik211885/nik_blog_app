using Application.Entities;

namespace Application.ValueObject;
/// <summary>
///     Navigation relationship n to n between post and subject   
/// </summary>
public class PostSubject
{
    /// <summary>
    ///  Key with post id it is foreign key to post
    /// </summary>
    public Guid PostId { get; set; }
    /// <summary>
    /// Key with subject id it is foreign key to post  
    /// </summary>
    public Guid SubjectId { get; set; }
    /// <summary>
    ///  Navigation to post object make EF can solve 
    /// </summary>
    public Post Posts { get; set; }
    /// <summary>
    ///  Navigation to subject object make EF can solve  
    /// </summary>
    public Subject Subjects { get; set; }
}