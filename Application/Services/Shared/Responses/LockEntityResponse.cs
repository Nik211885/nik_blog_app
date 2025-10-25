using System.Text.Json.Serialization;
using Application.ValueObject;

namespace Application.Services.Shared.Responses;

public class LockEntityResponse
{
    public bool Lock { get; init; }
    public string? LockReason { get; init; }
    public DateTimeOffset? LockToEnd { get; init; }
    public DateTimeOffset? LockToStart { get; init; }
};


internal static class LockEntityResponseExtension
{
    public static LockEntityResponse MapToResponse(this LockEntity @lock)
    {
        return new LockEntityResponse()
        {
            Lock = @lock.IsLock,
            LockReason = @lock.ReasonLock,
            LockToEnd = @lock.LockToTime,
            LockToStart = @lock.LockToTime,
        };
    } 
}