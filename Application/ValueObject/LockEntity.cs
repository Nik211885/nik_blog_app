namespace Application.ValueObject;
/// <summary>
///  Defined look entity
/// </summary>
public class LockEntity
{
    /// <summary>
    ///  Flag check to look entity
    /// </summary>
    public bool IsLock { get; set; } = false;
    /// <summary>
    /// Reason why entity has lock
    /// </summary>
    public string? ReasonLock { get; set; }
    /// <summary>
    ///  Lock at time
    /// </summary>
    public DateTimeOffset LockedAt { get; set; } = DateTimeOffset.UtcNow;
    /// <summary>
    ///  Lock entity to time
    /// </summary>
    public DateTimeOffset? LockToTime { get; set; }
    // If lock entity
}