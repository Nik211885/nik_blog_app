

using Application.Services.UserManager.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Application.Services.UserManager.Requests;
public class ResetPasswordRequest
{
    [Password(ErrorMessage = UserManageMessageConst.PasswordIsValidFormat)]
    public string Password { get; set; } = string.Empty;
    [Compare(nameof(Password), ErrorMessage = UserManageMessageConst.PasswordNotMatchConfirm)]
    public string PasswordConfirm { get; set; } = string.Empty;
}

