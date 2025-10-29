using Application.Entities;
using Application.Exceptions;
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
}
