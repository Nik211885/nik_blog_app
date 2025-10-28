using Application.Entities;

namespace Application.Repositories;

public interface IArgumentRepository
    : IRepositoryBase<Arguments>
{
    /// <summary>
    ///     Get argument by code
    /// </summary>
    /// <param name="code">code specific</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns>
    ///     Return argument if match the code otherwise null value
    /// </returns>
    Task<Arguments?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    /// <summary>
    ///     Get argument by id
    /// </summary>
    /// <param name="id">id specific</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns>
    ///     Return argument if match the id otherwise null value
    /// </returns>
    Task<Arguments?> GetByIdAsync(Guid id,  CancellationToken cancellationToken = default);
}