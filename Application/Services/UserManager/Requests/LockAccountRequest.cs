using Application.Entities;
using Application.ValueObject;

namespace Application.Services.UserManager.Requests;

public class LockAccountRequest
{
    public Guid UserId { get; init; }
    public string? ReasonLock { get; init; }
    public DateTimeOffset LockToTime { get; init; }
}

public static class LockAccountRequestExtension
{
    public static void MapToUser(this LockAccountRequest request, User user)
    {
        user.LockAccount ??= new LockEntity();
        user.LockAccount.IsLock = true;
        user.LockAccount.ReasonLock = request.ReasonLock;
        user.LockAccount.LockToTime = request.LockToTime;
        user.LockAccount.LockedAt = DateTimeOffset.UtcNow;
    }
}