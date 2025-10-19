namespace Application.Entities;
/// <summary>
///  Defined audit entity
/// </summary>
public abstract class AuditEntity : BaseEntity
{
    /// <summary>
    ///     Created data by user has id 
    /// </summary>
    public Guid CreatedBy { get; set; }
    /// <summary>
    ///     Modified data by user has id and first time modified by will is created by 
    /// </summary>
    public Guid ModifiedBy { get; set; }
    /// <summary>
    ///     Date time has created object and first time modified date will is created time
    /// </summary>
    public DateTimeOffset ModifiedAt { get; set; }
    /// <summary>
    ///  Define user has created 
    /// </summary>
    public User CreatedByUser { get; set; }
    /// <summary>
    /// Define user has modified 
    /// </summary>
    public User ModifiedByUser { get; set; }
}