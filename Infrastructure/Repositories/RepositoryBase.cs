using Application.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
    where TEntity : class, new()
{
    private readonly ApplicationDbContext _context;

    public RepositoryBase(ApplicationDbContext context)
    {
        _context = context;
    }
    /// <summary>
    ///     Change state entity is added
    /// </summary>
    /// <param name="entity"></param>
    public void Add(TEntity entity)
    {
        _context.Add(entity);
    }
    /// <summary>
    ///  Change state for entity is updated
    /// </summary>
    /// <param name="entity"></param>
    public void Update(TEntity entity)
    {
        _context.Update(entity);
    }
    /// <summary>
    ///  Change state for entity is deleted
    /// </summary>
    /// <param name="entity"></param>
    public void Delete(TEntity entity)
    {
        _context.Remove(entity);
    }
}