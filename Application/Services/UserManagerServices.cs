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
}