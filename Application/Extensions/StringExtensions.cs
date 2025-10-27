using Application.Helpers;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Application.Extensions;
/// <summary>
///  Contains all behavior extension is static method
/// </summary>
internal static class StringExtensions
{
    /// <summary>
    ///  Generator slug for value 
    /// </summary>
    /// <param name="value">value pares to slug unique value</param>
    /// <returns>Return slug friendly for url</returns>
    public static string GeneratorSlug(this string value)
    {
        string first = StringHelper.GeneratorRandomStringBase64(8);
        string normalized = value.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();
        var character = normalized.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark);
        sb.Append(character);
        string second = sb.ToString().Normalize(NormalizationForm.FormC).ToLowerInvariant();
        second = Regex.Replace(second, @"[^a-z0-9\s-]", "");
        second = Regex.Replace(second, @"\s+", "-").Trim('-');
        return string.Concat(first, "/", second);
    }
}
