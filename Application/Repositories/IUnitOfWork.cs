namespace Application.Repositories;
/// <summary>
///     Manage state for all repository
/// </summary>
/// You can reference to wrapper all instance repository in unit of work
/// or manual create all instance with repository in here
/// but in here make to simple i just make unit of work just is containing method manage state
public interface IUnitOfWork : IAsyncDisposable
{
    /// <summary>
    ///     Create transaction 
    /// </summary>
    /// <param name="cancellationToken">token to dispose action</param>
    /// <returns></returns>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    /// <summary>
    ///     Migration all rehouse has mark change to database
    /// </summary>
    /// <param name="cancellationToken">token to dispose action</param>
    /// <returns></returns>
    Task<int> SaveChangeAsync(CancellationToken cancellationToken = default);
    /// <summary>
    ///     Commit transaction to database
    /// </summary>
    /// <param name="cancellationToken">token to dispose action</param>
    /// <returns></returns>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    /// <summary>
    ///     Rollback all data to database
    /// </summary>
    /// <param name="cancellationToken">token to dispose action</param>
    /// <returns></returns>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}