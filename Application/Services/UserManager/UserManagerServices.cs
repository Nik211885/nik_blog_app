using Application.Adapters;
using Application.Entities;
using Application.Exceptions;
using Application.Extensions;
using Application.Helpers;
using Application.Repositories;
using Application.Services.UserManager.Requests;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Application.Services.UserManager.Models;

namespace Application.Services.UserManager;

public class UserManagerServices
{
    private readonly ILogger<UserManagerServices> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenEncryptionService _tokenEncryptionService;
    public UserManagerServices(ILogger<UserManagerServices> logger,
        IUnitOfWork unitOfWork, ITokenEncryptionService tokenEncryptionService)
    {
        _tokenEncryptionService = tokenEncryptionService;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    /// <summary>
    ///  Create new user with username and email is unique
    /// </summary>
    /// <param name="request">information for user need create for new instance</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns>
    ///    Return user response with information if user has created success
    /// </returns>
    public async Task<User> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        // validation with unique value
        User? user = await _unitOfWork.UserRepository
            .GetByUsernameAsync(request.UserName, cancellationToken);
        ThrowHelper.ThrowBusinessErrorWhenExitsItem(user, UserManageMessageConst.UserNameIsExits);

        List<User> users = await _unitOfWork.UserRepository
            .GetByEmailAsync(request.UserName, cancellationToken);

        user = users.FirstOrDefault();
        // Check exits with user find by email request if find user in system match email 
        // and email has confirmed => user has exits throw with exception
        // if user not confirm => have multiple account with email address
        // and when user confirm email => remove all account has duplicate email address
        ThrowHelper.ThrowBusinessErrorWhenExitsItem(user, UserManageMessageConst.EmailIsExits);

        user = request.MapToUser();
        user.UserCvSlug = user.UserCvSlug.GeneratorSlug();
        user.SecurityStamp = GetSecurityStampValue;
        if (request.Password is not null)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
        }
        // Send mail to confirm email you can write here or use ideal with eda
        _unitOfWork.UserRepository.Add(user);
        await _unitOfWork.SaveChangeAsync(cancellationToken);
        return user;
    }

    /// <summary>
    ///  Update information for user
    /// </summary>
    /// <param name="userId">user id need update</param>
    /// <param name="request">request with information from user</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns>
    ///     Return information user after update success
    /// </returns>
    public async Task<User> UpdateUserAsync(Guid userId, UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        User? user = await _unitOfWork
            .UserRepository.GetByIdAsync(userId, cancellationToken);
        ThrowHelper.ThrowWhenNotFoundItem(user, UserManageMessageConst.UserNotFound);

        request.MapToUser(user);
        user.SecurityStamp = GetSecurityStampValue;
        await _unitOfWork.SaveChangeAsync(cancellationToken);
        return user;
    }
    /// <summary>
    ///  Lock account
    /// </summary>
    /// <param name="request">reason why lock account</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns>
    ///     Return user after lock
    /// </returns>

    public async Task<User> LockAccountAsync(LockAccountRequest request,
        CancellationToken cancellationToken = default)
    {
        User? user = await _unitOfWork
            .UserRepository.GetByIdAsync(request.UserId, cancellationToken);
        ThrowHelper.ThrowWhenNotFoundItem(user, UserManageMessageConst.UserNotFound);

        UserManagerBusinessRule.CreateRule(user)
             .CheckLockAccount();

        request.MapToUser(user);
        user.SecurityStamp = GetSecurityStampValue;
        await _unitOfWork.SaveChangeAsync(cancellationToken);
        return user;
    }
    /// <summary>
    ///     Unlock account
    /// </summary>
    /// <param name="userId">user id need unlock account</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns>
    ///    Return user after unlock
    /// </returns>
    public async Task<User> UnLockAccountAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        User? user = await _unitOfWork
            .UserRepository.GetByIdAsync(userId, cancellationToken);
        ThrowHelper.ThrowWhenNotFoundItem(user, UserManageMessageConst.UserNotFound);

        UserManagerBusinessRule.CreateRule(user)
             .CheckLockAccount();
        // Ensure after check lock entity not null here you can move to method into member class for user
        // and use attribute MemberNotNull 
        user.LockAccount!.IsLock = false;
        user.LockAccount.LockToTime = null;
        user.SecurityStamp = GetSecurityStampValue;
        await _unitOfWork.SaveChangeAsync(cancellationToken);
        return user;
    }
    /// <summary>
    ///     Confirm email address for user has user id if email has confirmed throw exception business 
    ///     email has confirmed
    /// </summary>
    /// <param name="userId">user id represent for user need confirm email address</param>
    /// <param name="token">token to ensure user has required confirm email</param>
    /// <param name="cancellation">token to cancellation action</param>
    public async Task ConfirmEmailAsync(Guid userId, string token, CancellationToken cancellation = default)
    {
        User? user = await _unitOfWork.UserRepository.GetByIdAsync(userId, cancellation);
        ThrowHelper.ThrowWhenNotFoundItem(user, UserManageMessageConst.UserNotFound);
        if (user.Email is null || user.EmailConfirmed)
        {
            ThrowHelper.ThrowWhenBusinessError(UserManageMessageConst.EmailAddressHasConfirm);
        }
        UserManagerBusinessRule.CreateRule(user)
            .ValidUserToken(DecodeUserToken(token), UserTokenType.ConfirmEmail);
        
        user.EmailConfirmed = true;
        user.SecurityStamp = GetSecurityStampValue;
        // Find when all user has email address and delete all user don't have confirm
        List<User> users = await _unitOfWork.UserRepository.GetByEmailAsync(user.Email, cancellation, isConfirm: false);
        // It just simple loop with each entity and add state is deleted 
        // it will have problem about performance => you can use bulk delete with condition delete for that
        foreach (var u in users.Where(x => x.Id != user.Id))
        {
            _unitOfWork.UserRepository.Delete(u);
        }
        await _unitOfWork.SaveChangeAsync(cancellation);
    }
    /// <summary>
    ///   Reset password for user update new password
    /// </summary>
    /// <param name="userId">user need update password</param>
    /// <param name="request">includes password and password confirm</param>
    /// <param name="token">token to ensure user has required to be reset password</param>
    /// <param name="cancellation">token to cancellation action</param>
    public async Task ResetPasswordAsync(Guid userId, string token, ResetPasswordRequest request
        , CancellationToken cancellation = default)
    {
        User? user = await _unitOfWork.UserRepository.GetByIdAsync(userId, cancellation);
        ThrowHelper.ThrowWhenNotFoundItem(user, UserManageMessageConst.UserNotFound);

        UserManagerBusinessRule.CreateRule(user)
            .ValidUserToken(DecodeUserToken(token), UserTokenType.ResetPassword)
            .CheckLockAccount()
            .CheckPassword(request.Password);

        user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
        user.SecurityStamp = GetSecurityStampValue;
        _unitOfWork.UserRepository.Update(user);
        await _unitOfWork.SaveChangeAsync(cancellation);
    }
    /// <summary>
    ///  Generator token with user manager
    /// </summary>
    /// <param name="user">information user encrypt</param>
    /// <param name="tokenType">token type is action with verify</param>
    /// <returns>
    ///     Return token with base 64 encrypt data about user information
    /// </returns>
    public string GeneratorUserToken(User user, UserTokenType tokenType, int minuteExpriedToken = 1)
    {
        var userPayload = new UserPayloadToken()
        {
            UserId = user.Id.ToString(),
            UserName = user.UserName,
            SecurityStamp = user.SecurityStamp,
            TokenExpired = DateTimeOffset.UtcNow.AddMinutes(minuteExpriedToken),
            TokenType = tokenType,
        };
        string jsonUserPayload = JsonSerializer.Serialize(userPayload);
        string token = _tokenEncryptionService.Encrypt(jsonUserPayload);
        return token;
    }
    /// <summary>
    ///     Verify token is compare all value in payload with information match user 
    /// </summary>
    /// <param name="token">token need to check</param>
    /// <returns>
    ///     Return UserPayloadToken
    /// </returns>
    private UserPayloadToken DecodeUserToken(string token)
    {
        string jsonTokenPayload = _tokenEncryptionService.Decrypt(token);
        UserPayloadToken? userPayloadToken = JsonSerializer.Deserialize<UserPayloadToken>(jsonTokenPayload);
        ThrowHelper.ThrowWhenBusinessError(UserManageMessageConst.InvalidUserToken);
        return userPayloadToken;
    }
    /// <summary>
    ///  Generator security stamp value random is string base 64 with length 32 bits
    /// </summary>
    private static string GetSecurityStampValue => StringHelper.GeneratorRandomStringBase64(32);
}