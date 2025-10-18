namespace Application.Entities;
/// <summary>
///     Defined base entity
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Identify entity  it is unique for each entity with guid idv7
    /// </summary>
    public Guid Id { get; } = Guid.CreateVersion7();
    /// <summary>
    ///     Created data by user has id 
    /// </summary>
    public string CreatedBy { get; set; }
    /// <summary>
    ///     Date time has created object
    /// </summary>
    public DateTimeOffset CreatedAt { get; } = DateTimeOffset.UtcNow;
    /// <summary>
    ///     Modified data by user has id and first time modified by will is created by 
    /// </summary>
    public string ModifiedBy { get; set; }
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