using Application.Entities;
using Application.Exceptions;
using Application.ValueObject;
using System.Diagnostics.CodeAnalysis;

namespace Application.Services.UserManager;
internal class UserManagerBusinessRule
{
    private readonly User _user;
    private UserManagerBusinessRule(User user)
    {
        _user = user;
    }
    /// <summary>
    ///  Check user account has lock throw business error when account is lock
    ///  and after call method lock entity can not be null you can use ! specific lock entity talk 
    ///  for compiler value can not be null
    /// </summary>
    public UserManagerBusinessRule CheckLockAccount()
    {
        LockEntity? lockAccount = _user.LockAccount;

        if (lockAccount is null || lockAccount.IsLock)
        {
            ThrowHelper.ThrowWhenBusinessError(UserManageMessageConst.AccountHasLock);
        }

        return this;
    }
    /// <summary>
    ///      Check if new password like with old password throw business error for that
    ///      if user login with oath2 don't have password it will don't catch this case
    /// </summary>
    /// <param name="newPassword">password user need update</param>
    /// <returns></returns>
    public UserManagerBusinessRule NewPasswordCanNotLikeOldPassword(string newPassword)
    {
        if (!string.IsNullOrWhiteSpace(_user.Password) && BCrypt.Net.BCrypt.Verify(newPassword, _user.Password))
        {
            ThrowHelper.ThrowWhenBusinessError(UserManageMessageConst.NewPasswordCanNotLikeOldPassword);
        }   
        return this;
    }
    /// <summary>
    ///  Create new instance for user manager business rule
    /// </summary>
    /// <param name="user">user instance</param>
    /// <returns>
    ///     Return instance for user manager bussiness with user has specific 
    /// </returns>
    public static UserManagerBusinessRule CreateRule(User user)
    {
        return new UserManagerBusinessRule(user);
    }
}