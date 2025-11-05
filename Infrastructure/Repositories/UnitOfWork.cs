using Application.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Repositories;

public class UnitOfWork : RepositoryWrapper, IUnitOfWork
{
    /// <summary>
    ///     Instance for application db context
    /// </summary>
    private readonly ApplicationDbContext _context;
    /// <summary>
    ///  Services provider is get instance for di container
    /// </summary>
    private readonly IServiceProvider _serviceProvider;
    /// <summary>
    ///     Current transaction in unit of work
    /// </summary>
    private IDbContextTransaction? _transaction;

    public UnitOfWork(ApplicationDbContext context, IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        _context = context;
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    ///     Create new transaction
    /// </summary>
    /// <param name="cancellationToken">token to cancellation action</param>
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction ??= await _context.Database.BeginTransactionAsync(cancellationToken);
    }
    /// <summary>
    ///     Save change, it calls base method for db context is migration all state to database
    /// </summary>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns></returns>
    public Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
    /// <summary>
    ///    Create commit command to database it will call save change after commit transaction
    /// </summary>
    /// <param name="cancellationToken">token to cancellation action</param>
    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_transaction);
        try
        {
            await this.SaveChangeAsync(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await _transaction.RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
    /// <summary>
    ///  Roll back all data if have exception in process migration data to database
    /// </summary>
    /// <param name="cancellationToken">token to cancellation action</param>
    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_transaction);
        await _transaction.RollbackAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public ValueTask DisposeAsync()
    {
        return _context.DisposeAsync();
    }
}