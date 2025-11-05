using System.ComponentModel.DataAnnotations;
using Application.Services.UserManager.Attributes;

namespace Application.Services.SignInManager.Requests;

public class PasswordLoginRequest
{
    [Required(ErrorMessage = SignInConstMessage.UserNameCanNotBeNull)]
    public string UserName { get; init; } = string.Empty;
    [Password(ErrorMessage = SignInConstMessage.PasswordInval)]
    public string Passsword { get; init; } = string.Empty;
}
