using Application.Adapters;
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
    public async Task<UserResponse> CreateUser(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        // validation with unique value
        User? user = await _unitOfWork.UserRepository
            .GetByUsernameAsync(request.UserName, cancellationToken);
        ThrowHelper.ThrowBusinessErrorWhenExitsItem(user, UserManageMessageConst.UserNameIsExits);
        
        user = await _unitOfWork.UserRepository
            .GetByEmailAsync(request.UserName, cancellationToken);
        ThrowHelper.ThrowBusinessWithCondition(user,u=>(u is not null && u.EmailConfirmed), UserManageMessageConst.UserNameIsExits);

        user = request.MapToUser();
        user.UserCvSlug = user.UserCvSlug.GeneratorSlug();
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        
        // Send mail to confirm email you can write here or use ideal with eda
        _unitOfWork.UserRepository.Add(user);
        await _unitOfWork.SaveChangeAsync(cancellationToken);
        return user.MapToResponse();
    }
}