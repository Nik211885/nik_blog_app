using Application.Entities;
using Application.Enums;

namespace Application.Services.SignInManager.Responses;

public class UserSignResponse
{
    public Guid UserId { get; init; }
    public string UserName { get; init; } = string.Empty;
    public string? EmailAddress { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? FullName { get; init; }
    public Role Role { get; init; }
}

public static class UserSignResponseExtension
{
    public static UserSignResponse MapToSignResponse(this User user)
    {
        return new UserSignResponse()
        {
            UserId = user.Id,
            UserName = user.UserName,
            EmailAddress = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            FullName = user.FullName,
            Role = user.Role,
        };
    }
}
