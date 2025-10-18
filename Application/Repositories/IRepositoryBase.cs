namespace Application.Repositories;
/// <summary>
///  Base for repository it use to scan and register to di container
/// and contains base method for each repository 
/// </summary>
public interface IRepositoryBase<TEntity>
{
    /// <summary>
    ///     Get entity has id  
    /// </summary>
    /// <param name="id">id for user</param>
    /// <param name="cancellationToken">token to cancellation</param>
    /// <returns>
    ///     Return entity if find entity has id exits
    ///     Otherwise return null value
    /// </returns>
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    /// <summary>
    ///     Update state for entity is added
    /// </summary>
    /// <param name="entity"></param>
    void Add(TEntity entity);
    /// <summary>
    ///   Update state for entity is updated  
    /// </summary>
    /// <param name="entity"></param>
    void Update(TEntity entity);
    /// <summary>
    ///    Update state for entity is deleted
    /// </summary>
    /// <param name="entity"></param>
    void Delete(TEntity entity);
}