using Application.Entities;
using Application.Exceptions;
using Application.ValueObject;
using Application.Services.UserManager.Models;

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
        if (CheckLockAccountBoolean())
        {
            ThrowHelper.ThrowWhenBusinessError(UserManageMessageConst.AccountHasLock);
        }

        return this;
    }
    /// <summary>
    ///  Check account has lock
    /// </summary>
    /// <returns>
    ///     Return true if account has lock otherwise false
    /// </returns>
    public bool CheckLockAccountBoolean()
    {
        LockEntity? lockAccount = _user.LockAccount;
        return lockAccount is not null && lockAccount.IsLock;
    }
    /// <summary>
    ///      Check if new password like with old password throw business error for that
    ///      if user login with oath2 don't have password it will don't catch this case
    /// </summary>
    /// <param name="newPassword">password user need update</param>
    /// <returns></returns>
    public UserManagerBusinessRule CheckPassword(string newPassword)
    {
        if (CheckPasswordBoolean(newPassword))
        {
            ThrowHelper.ThrowWhenBusinessError(UserManageMessageConst.NewPasswordCanNotLikeOldPassword);
        }
        return this;
    }
    /// <summary>
    ///     Check password correct with user password
    /// </summary>
    /// <param name="newPassword">password need check with user</param>
    /// <returns>
    ///     Return true if password match to user otherwise false
    /// </returns>
    public bool CheckPasswordBoolean(string newPassword)
    {
        return !string.IsNullOrWhiteSpace(_user.Password) && BCrypt.Net.BCrypt.Verify(newPassword, _user.Password);
    }
    /// <summary>
    /// Check valid token with user
    /// </summary>
    /// <param name="userPayloadToken">token after decode</param>
    /// <param name="userTokenType">type of token</param>
    /// <returns>Return this rule instance</returns>
    public UserManagerBusinessRule ValidUserToken(UserPayloadToken userPayloadToken, UserTokenType userTokenType)
    {
        if (userPayloadToken.TokenExpired < DateTime.UtcNow
            || userPayloadToken.UserId != _user.Id.ToString()
            || userPayloadToken.SecurityStamp != _user.SecurityStamp
            || userPayloadToken.TokenType != userTokenType
            || userPayloadToken.UserName != _user.UserName
            )
        {
            ThrowHelper.ThrowWhenBusinessError(UserManageMessageConst.InvalidUserToken);
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