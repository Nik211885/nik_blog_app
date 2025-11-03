using Application.Entities;
using Application.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ArgumentRepository
    : RepositoryBase<Arguments>, IArgumentRepository
{
    private readonly ApplicationDbContext _context;

    public ArgumentRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    /// <summary>
    ///     Get argument by code
    /// </summary>
    /// <param name="code">code specific</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns>
    ///     Return argument if match the code otherwise null value
    /// </returns>
    public async Task<Arguments?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        Arguments? argument = await _context.Arguments.
                FirstOrDefaultAsync(x=>x.Code == code, cancellationToken);
        return argument;
    }
    /// <summary>
    ///     Get argument by id
    /// </summary>
    /// <param name="id">id specific</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns>
    ///     Return argument if match the id otherwise null value
    /// </returns>

    public  async Task<Arguments?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Arguments? argument = await _context.Arguments.
            FirstOrDefaultAsync(x=>x.Id == id, cancellationToken);
        return argument;
    }
    /// <summary>
    ///     Create all instance argument match with ids
    /// </summary>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <param name="ids">ids match to find argument</param>
    /// <returns>
    ///     Return list of argument match ids 
    /// </returns>
    public async Task<IEnumerable<Arguments>> GetByIdsAsync(CancellationToken cancellationToken = default, params Guid[] ids)
    {
        List<Arguments> arguments = await _context.Arguments.Where(x => ids.Contains(x.Id))
                                    .ToListAsync(cancellationToken);
        return arguments;
    }
}