using Application.ValueObject;

namespace Application.Entities;
/// <summary>
///     Defined model subject to group post
/// </summary>
public class Subject : BaseEntity
{
    /// <summary>
    ///  Name for subject
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    ///  Slug for friendly for access subject
    /// </summary>
    public string Slug { get; set; }
    /// <summary>
    /// Use to sort subject
    /// </summary>
    public int OrderIndex { get; set; }
    /// <summary>
    ///     Id for parent subject
    /// </summary>
    public Guid SubjectParentId { get; set; }
    /// <summary>
    ///   Flags to know subject will display to navigation  blog
    /// </summary>
    public bool IsDisplayNavigation { get; set; }
    /// <summary>
    ///   Lock for subject
    /// </summary>
    public LockEntity LockSubject {get; set;}
    /// <summary>
    /// Collection post to subject navigation for ef
    /// </summary>
    public ICollection<PostSubject> PostSubjects { get; set; }  
    /// <summary>
    ///  Navigation for parent subject
    /// </summary>
    public Subject SubjectParent { get; set; }
    /// <summary>
    ///  Collection child subject
    /// </summary>
    public ICollection<Subject> ChildSubject { get; set; }
}