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
    ///     Date time has created object
    /// </summary>
    public DateTimeOffset CreatedAt { get; } = DateTimeOffset.UtcNow;
}