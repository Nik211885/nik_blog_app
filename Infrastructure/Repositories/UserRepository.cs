using Application.Entities;
using Application.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    private readonly ApplicationDbContext _context;
    public UserRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    /// <summary>
    ///     Get user by id
    /// </summary>
    /// <param name="id">id for user</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns>
    ///     Return user if match id otherwise is null
    /// </returns>
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        User? userById = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return userById;
    }
    /// <summary>
    /// Get user by email
    /// </summary>
    /// <param name="email">email for user</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns>
    /// Return user if match email otherwise is null
    /// </returns>
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        User? userByEmail = await _context.Users
            .FirstOrDefaultAsync(x => x.Email == email,cancellationToken);
        return userByEmail;
    }
    /// <summary>
    /// Get user by username
    /// </summary>
    /// <param name="userName">username for user</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns>
    /// Return user if match username otherwise is null
    /// </returns>
    public async Task<User?> GetByUsernameAsync(string userName, CancellationToken cancellationToken = default)
    {
        User? userByUserName = await _context
            .Users
            .FirstOrDefaultAsync(x => x.UserName == userName, cancellationToken);
        return userByUserName;
    }
    /// <summary>
    /// Get user by phone number
    /// </summary>
    /// <param name="phoneNumber">phone number for user</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns>
    /// Return user if match phone number otherwise is null
    /// </returns>
    public async Task<User?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        User? userByPhoneNumber = await _context
            .Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber, cancellationToken);
        return userByPhoneNumber;
    }
}