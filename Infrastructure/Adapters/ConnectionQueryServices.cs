
using System.Data;
using Application.Adapters;
using Dapper;

namespace Infrastructure.Adapters;

public class ConnectionQueryServices : IConnectionQueryService
{
    private readonly IDbConnection _dbConnection;
    public ConnectionQueryServices(IDbConnection connection)
    {
        _dbConnection = connection;
    }
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
    public Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null)
    {
        return _dbConnection.QueryAsync<T>(sql, param, transaction);
    }
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
    public Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null)
    {
        return _dbConnection.QueryFirstOrDefaultAsync<T>(sql, param, transaction);
    }
}
