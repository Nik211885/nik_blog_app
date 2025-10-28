﻿namespace Application.Services.UserManager.Models;
public enum UserTokenType
{
    /// <summary>
    ///  Type token to confirm email
    /// </summary>
    ConfirmEmail,
    /// <summary>
    ///    Type token to reset password
    /// </summary>
    ResetPassword,
}
