using Application.Adapters;
using Application.Entities;
using Application.Exceptions;
using Application.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class UserManagerServices
{
    private readonly ILogger<UserManagerServices> _logger;
    private readonly IUnitOfWork _unitOfWork;
    public UserManagerServices(ILogger<UserManagerServices> logger, 
        IUnitOfWork unitOfWork, IUserProvider userProvider)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    /// <summary>
    ///     Create new user
    /// </summary>
    /// <param name="user">user</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns>
    ///     Return instance for user if create success it otherwise throws exception  
    /// </returns>
    public async Task<User> CreateUserAsync(User user,
        CancellationToken cancellationToken = default)
    {
        User? userByUserName = await _unitOfWork
            .UserRepository.GetByUsernameAsync(user.UserName, cancellationToken);
        ThrowHelper.ThrowBusinessErrorWhenExitsItem(userByUserName, ConstErrorMessage.ExitUserName);
        
        if (user.Email != null)
        {
            User? userByEmail = await _unitOfWork.UserRepository
                .GetByEmailAsync(user.Email, cancellationToken);
            ThrowHelper.ThrowBusinessErrorWhenExitsItem(userByEmail, ConstErrorMessage.ExitEmailAddress);
        }

        if (user.PhoneNumber != null)
        {
            User? userByPhone = await _unitOfWork.UserRepository
                .GetByPhoneNumberAsync(user.PhoneNumber, cancellationToken);
            ThrowHelper.ThrowBusinessErrorWhenExitsItem(userByPhone, ConstErrorMessage.ExitPhoneNumber);
        }

        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        _unitOfWork.UserRepository.Add(user);
        await _unitOfWork.SaveChangeAsync(cancellationToken);
        return user;
    }
}