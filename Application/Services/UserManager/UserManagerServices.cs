using Application.Entities;
using Application.Exceptions;
using Application.Helpers;
using Application.Repositories;
using Application.Services.UserManager.Requests;
using Application.Services.UserManager.Responses;
using Microsoft.Extensions.Logging;

namespace Application.Services.UserManager;

public class UserManagerServices
{
    private readonly ILogger<UserManagerServices> _logger;
    private readonly IUnitOfWork _unitOfWork;
    public UserManagerServices(ILogger<UserManagerServices> logger, 
        IUnitOfWork unitOfWork)
    {
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
    public async Task<UserResponse> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        // validation with unique value
        User? user = await _unitOfWork.UserRepository
            .GetByUsernameAsync(request.UserName, cancellationToken);
        ThrowHelper.ThrowBusinessErrorWhenExitsItem(user, UserManageMessageConst.UserNameIsExits);
        
        user = await _unitOfWork.UserRepository
            .GetByEmailAsync(request.UserName, cancellationToken);
        if (user is not null && user.EmailConfirmed)
        {
            ThrowHelper.ThrowWhenBusinessError(UserManageMessageConst.EmailIsExits);   
        }

        user = request.MapToUser();
        user.UserCvSlug = user.UserCvSlug.GeneratorSlug();
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        
        // Send mail to confirm email you can write here or use ideal with eda
        _unitOfWork.UserRepository.Add(user);
        await _unitOfWork.SaveChangeAsync(cancellationToken);
        return user.MapToResponse();
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
    public async Task<UserResponse> UpdateUserAsync(Guid userId, UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        User? user = await _unitOfWork
            .UserRepository.GetByIdAsync(userId, cancellationToken);
        ThrowHelper.ThrowWhenNotFoundItem(user, UserManageMessageConst.UserNotFound);

        request.MapToUser(user);
        await _unitOfWork.SaveChangeAsync(cancellationToken);
        return user.MapToResponse();
    }
    /// <summary>
    ///  Lock account
    /// </summary>
    /// <param name="request">reason why lock account</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns>
    ///     Return user after lock
    /// </returns>

    public async Task<UserResponse> LockAccountAsync(LockAccountRequest request,
        CancellationToken cancellationToken = default)
    {
        User? user = await _unitOfWork
            .UserRepository.GetByIdAsync(request.UserId, cancellationToken);
        ThrowHelper.ThrowWhenNotFoundItem(user, UserManageMessageConst.UserNotFound);

        if (user.LockAccount is not null && user.LockAccount.IsLock)
        {
            ThrowHelper.ThrowWhenBusinessError(UserManageMessageConst.AccountHasLock);
        }

        request.MapToUser(user);
        await _unitOfWork.SaveChangeAsync(cancellationToken);
        return user.MapToResponse();
    }
    /// <summary>
    ///     Unlock account
    /// </summary>
    /// <param name="userId">user id need unlock account</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns>
    ///    Return user after unlock
    /// </returns>
    public async Task<UserResponse> UnLockAccountAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        User? user = await _unitOfWork
            .UserRepository.GetByIdAsync(userId, cancellationToken);
        ThrowHelper.ThrowWhenNotFoundItem(user, UserManageMessageConst.UserNotFound);
        if (user.LockAccount is null || !user.LockAccount.IsLock)
        {
            ThrowHelper.ThrowWhenBusinessError(UserManageMessageConst.AccountHasUnLock);
        }

        user.LockAccount.IsLock = false;
        user.LockAccount.LockToTime = null;
        await _unitOfWork.SaveChangeAsync(cancellationToken);
        return user.MapToResponse();
    }
}