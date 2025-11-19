using System.Linq.Expressions;
using Infrastructure.DataModels;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Extensions;
/// <summary>
///     Extension queries for queryable
/// </summary>
public static class QueriesExtensions
{
    /// <summary>
    ///     Get pagination item with queryable
    /// </summary>
    /// <param name="queryable">queryable to get pagination</param>
    /// <param name="pageNumber">page number</param>
    /// <param name="pageSize">page size</param>
    /// <param name="cancellationToken">cancellation token</param>
    /// <typeparam name="TResponse">Type of response</typeparam>
    /// <returns>
    ///     Return pagination data model with generic is TResponse
    /// </returns>
    public static async Task<PaginationResponse<TResponse>> GetPaginationAsync<TResponse>(
        IQueryable<TResponse> queryable,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default
        )
    {
        var totalPage = await queryable.CountAsync(cancellationToken);
        var paginationItem = await queryable
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        return new PaginationResponse<TResponse>(paginationItem, pageNumber, pageSize, totalPage);
    }
    /// <summary>
    ///     Simplfy way mapping expression in queryable from TEntity to TResponse 
    /// </summary>
    /// <param name="queryable"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns></returns>
    public static IQueryable<TResponse> MapTo<TEntity, TResponse>(this IQueryable<TEntity> queryable)
        where TResponse: new()
    {
        Expression<Func<TEntity, TResponse>> mappingExpression = ExpressionHelpers.MapTo<TEntity, TResponse>();
        return queryable.Select(mappingExpression);
    }
}