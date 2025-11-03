using System.Data;

namespace Application.Adapters;
/// <summary>
///  Expose some method query to database and simple mapping 
/// </summary>
public interface IConnectionQueryService
{
    /// <summary>
    ///     Query to map to list for generic of type t
    /// </summary>
    /// <typeparam name="T">generic for type T</typeparam>
    /// <param name="sql">command sql to query get list data</param>
    /// <param name="param">param in sql command</param>
    /// <param name="transaction">transaction</param>
    /// <returns>
    ///     Return list data of type T
    /// </returns>
    Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null);
    /// <summary>
    ///     Query and get first record for sql and otherwise return null if not match record
    /// </summary>
    /// <typeparam name="T">generic for type T</typeparam>
    /// <param name="sql">command sql to query get list data</param>
    /// <param name="param">param in sql command</param>
    /// <param name="transaction">transaction</param>
    /// <returns>
    ///     Return first record with sql otherwise is null value
    /// </returns>
    Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null);
}
