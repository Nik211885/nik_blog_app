using System.ComponentModel.DataAnnotations;
using Application.Entities;
using Application.Services.UserManager.Attributes;

namespace Application.Services.UserManager.Requests;

public class CreateUserRequest
{
    [Required(ErrorMessage = UserManageMessageConst.UserNameIsRequired)]
    [MaxLength(50, ErrorMessage = UserManageMessageConst.UserNameMaxLength)]
    public string UserName { get; init; } 

    [EmailAddress(ErrorMessage = UserManageMessageConst.EmailIsValidFormat)]
    public string Email { get; init; }

    [Password(ErrorMessage = UserManageMessageConst.PasswordIsValidFormat)]
    public string Password { get; init; }

    [Compare(nameof(Password), ErrorMessage = UserManageMessageConst.PasswordNotMatchConfirm)]
    public string PasswordConfirm { get; init; } 
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