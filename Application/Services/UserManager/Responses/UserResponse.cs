using Application.Entities;
using Application.Services.Shared.Responses;

namespace Application.Services.UserManager.Responses;

public class UserResponse
{ 
        public Guid Id { get; init; }
       public DateTimeOffset CreatedAt{ get; init; }
       public DateTimeOffset? UpdateAt{ get; init; }
       public string UserName{ get; init; } = string.Empty;
       public string? Email{ get; init; }
       public bool EmailConfirm{ get; init; }
       public string? FirstName{ get; init; }
       public string? LastName{ get; init; }
       public string? FullName{ get; init; }
       public string? PhoneNumber{ get; init; }
       public bool PhoneNumberConfirm{ get; init; }
       public string? AvatarUrl{ get; init; }
       public DateTimeOffset? BirthDay{ get; init; }
       public string? Bio{ get; init; }
       public string UserCvSlug{ get; init; } = string.Empty;
       public int LoginFail{ get; init; }
       public LockEntityResponse? LockUser{ get; init; }
};


internal static class UserResponseExtension
{
    public static UserResponse MapToResponse(this User user)
    {
        return new UserResponse()
        {
            Id = user.Id,
            CreatedAt = user.CreatedAt,
            UpdateAt = user.ModifiedAt,
            UserName = user.UserName,
            Email = user.Email,
            EmailConfirm = user.EmailConfirmed,
            FirstName = user.FirstName,
            LastName = user.LastName,
            FullName = user.FullName,
            PhoneNumber = user.PhoneNumber,
            PhoneNumberConfirm = user.PhoneNumberConfirmed,
            AvatarUrl = user.Avatar,
            BirthDay = user.BirthDay,
            Bio = user.Bio,
            UserCvSlug = user.UserCvSlug,
            LoginFail = user.CoutLoginFail,
            LockUser = user.LockAccount?.MapToResponse()
        };
    }
}