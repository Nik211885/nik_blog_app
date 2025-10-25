using Application.Entities;

namespace Application.Services.UserManager.Requests;

public class UpdateUserRequest
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? FullName { get; init; }
    public string? Avatar { get; init; }
    public DateTimeOffset? BirthDay { get; init; }
    public string? Bio { get; init; }
    public string UserCvSlug { get; init; } = string.Empty;
    
}

internal static class UpdateUserRequestExtension
{
    public static void MapToUser(this UpdateUserRequest request, User  user)
    {
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.FullName = request.FullName;
        user.Avatar = request.Avatar;
        user.BirthDay = request.BirthDay;
        user.Bio = request.Bio;
        user.UserCvSlug = request.UserCvSlug;
    }
}