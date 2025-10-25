using System.Globalization;
using System.Reflection.PortableExecutable;
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
    ///  Generator slug for value 
    /// </summary>
    /// <param name="value">value pares to slug unique value</param>
    /// <returns>Return slug friendly for url</returns>
    internal static string GeneratorSlug(this string value)
    {
        string first = GeneratorRandomStringBase64(8);
        string normalized= value.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();
        var character = normalized.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark);
        sb.Append(character);
        string second = sb.ToString().Normalize(NormalizationForm.FormC).ToLowerInvariant();
        second = Regex.Replace(second, @"[^a-z0-9\s-]", "");
        second = Regex.Replace(second, @"\s+", "-").Trim('-');
        return string.Concat(first, "/", second);
    }
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