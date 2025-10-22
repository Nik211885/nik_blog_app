using Application.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
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
        if (entity != null)
        {
            _context.Add(entity);
        }
    }
    /// <summary>
    ///  Change state for entity is updated
    /// </summary>
    /// <param name="entity"></param>
    public void Update(TEntity entity)
    {
        if (entity != null)
        {
            _context.Update(entity);
        }
    }
    /// <summary>
    ///  Change state for entity is deleted
    /// </summary>
    /// <param name="entity"></param>
    public void Delete(TEntity entity)
    {
        if (entity != null)
        {
            _context.Remove(entity);
        }
    }
}