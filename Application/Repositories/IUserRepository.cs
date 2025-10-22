using Application.Entities;

namespace Application.Repositories;

/// <summary>
///  Repository for user
/// </summary>
public interface IUserRepository : IRepositoryBase<User>
{
    /// <summary>
    ///     Get user by id
    /// </summary>
    /// <param name="id">id for user</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns>
    ///     Return user match id or return null if not find user match id
    /// </returns>
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    /// <summary>
    ///     Get user by email
    /// </summary>
    /// <param name="email">email for user</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns>
    ///     Return user match email or return null if not find user match email
    /// </returns>
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    /// <summary>
    /// Get user by username
    /// </summary>
    /// <param name="userName">username for user</param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     Return user match username or return null if not find user match username
    /// </returns>
    Task<User?> GetByUsernameAsync(string userName, CancellationToken cancellationToken = default);
    /// <summary>
    ///  Get user by phone number
    /// </summary>
    /// <param name="phoneNumber">phone number for user</param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///Return user match phone number or return null if not find user match phone number
    /// </returns>
    Task<User?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);
}