using System.Diagnostics.CodeAnalysis;

namespace Application.Exceptions;

public static class ThrowHelper
{
    /// <summary>
    ///     Throw unauthorized exception
    /// </summary>
    /// <param name="message">message for unauthorized</param>
    /// <exception cref="UnauthorizedException"></exception>
    [DoesNotReturn]
    public static void ThrowUnauthorized(string message)
    {
        throw new UnauthorizedException(message);
    }
}