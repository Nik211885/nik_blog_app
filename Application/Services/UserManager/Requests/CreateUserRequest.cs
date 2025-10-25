using System.ComponentModel.DataAnnotations;
using Application.Entities;
using Application.Services.UserManager.Attributes;

namespace Application.Services.UserManager.Requests;

public class CreateUserRequest
{
    [Required(ErrorMessage = UserManageMessageConst.UserNameIsRequired)]
    [MaxLength(50, ErrorMessage = UserManageMessageConst.UserNameMaxLength)]
    public string UserName { get; init; } = string.Empty;

    [EmailAddress(ErrorMessage = UserManageMessageConst.EmailIsValidFormat)]
    public string Email { get; init; } = string.Empty;

    [Password(ErrorMessage = UserManageMessageConst.PasswordIsValidFormat)]
    public string Password { get; init; } = string.Empty;

    [Compare(nameof(Password), ErrorMessage = UserManageMessageConst.PasswordNotMatchConfirm)]
    public string PasswordConfirm { get; init; }  = string.Empty;
}


internal static class CreateUserRequestExtension
{
    public static User MapToUser(this CreateUserRequest request)
    {
        return new User()
        {
            UserName = request.UserName,
            Email = request.Email,
        };
    }
}