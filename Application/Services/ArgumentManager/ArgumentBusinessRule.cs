using Application.Entities;
using Application.Exceptions;
using Dapper;
using System.Data;

namespace Application.Services.ArgumentManager;
/// <summary>
///     Defined argument rule 
/// </summary>
internal class ArgumentBusinessRule
{
    private readonly Arguments _arguments;
    private ArgumentBusinessRule(Arguments arguments)
    {
        _arguments = arguments;
    }
    /// <summary>
    ///     Create new instance for argument rule
    /// </summary>
    /// <param name="arguments">argument for instance rule</param>
    /// <returns>
    ///     Return instance for business rule with argument param
    /// </returns>
    public static ArgumentBusinessRule CreateRule(Arguments arguments)
    {
        return new ArgumentBusinessRule(arguments);
    }
    /// <summary>
    ///  Check query has invalid with arguments in query check query has success query to database
    /// </summary>
    public async Task CheckInvalidQueryAsync(IDbConnection dbConection)
    {
        Guid userId = Guid.NewGuid();
        string userName = "ninhlk";
        try
        {
            string? value = await dbConection.QueryFirstOrDefaultAsync<string>(_arguments.Query, new
            {
                userId = userId,
                userName = userName
            });
        }
        catch (Exception ex)
        {
            ThrowHelper.ThrowWhenBusinessError(ex.Message);   
        }        
    }
}
