namespace Application.Enums;
/// <summary>
///  Defined type services for notification defined for template
/// </summary>
public enum NotificationServicesType
{
    /// <summary>
    ///  Services create account
    /// </summary>
    CreateNewAccount,
    /// <summary>
    ///   Services change password
    /// </summary>
    ResetPassword,
    /// <summary>
    ///   Services confirm password
    /// </summary>
    ConfirmEmail,
    /// <summary>
    ///  Services when user create new account success by method is extend provider
    /// </summary>
    CreateNewAccountByExtendProvider,
}