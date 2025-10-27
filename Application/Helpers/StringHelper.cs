using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Application.Helpers;
/// <summary>
///  Helper for application layer
/// </summary>
public static class StringHelper
{
    /// <summary>
    ///     Generator random string base 64
    /// </summary>
    /// <param name="bytes">Number byte in random string</param>
    /// <returns>
    ///     Return string base 64 with length suit bytes
    /// </returns>
    public static string GeneratorRandomStringBase64(int bytes)
    {
        var rand = new byte[bytes];
        using (var random = RandomNumberGenerator.Create())
        {
            random.GetBytes(rand);
        }
        return Convert.ToBase64String(rand);
    }
}