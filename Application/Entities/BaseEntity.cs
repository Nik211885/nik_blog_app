namespace Application.Entities;
/// <summary>
///     Defined base entity
/// </summary>
public abstract class BaseEntity<TKey> where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Identify entity  it is unique for each entity
    /// </summary>
    public TKey Id { get; set; }
    /// <summary>
    ///     Created data by user has id 
    /// </summary>
    public string CreatedBy { get; set; }
    /// <summary>
    ///     Date time has created object
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }
    /// <summary>
    ///     Modified data by user has id and first time modified by will is created by 
    /// </summary>
    public string ModifiedBy { get; set; }
    /// <summary>
    ///     Date time has created object and first time modified date will is created time
    /// </summary>
    public DateTimeOffset ModifiedAt { get; set; }
}