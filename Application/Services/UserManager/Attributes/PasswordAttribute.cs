using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Application.Services.UserManager.Attributes;

/// <summary>
///  Attribute for valid password format with pattern
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class PasswordAttribute : ValidationAttribute
{
    private readonly string _regexPasswordFormat = "^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$";

    public override bool IsValid(object? value)
    {
        if (value is not string password)
        {
            return true;
        }

        Match valid = Regex.Match(password, _regexPasswordFormat);
        return valid.Success;
    }
}