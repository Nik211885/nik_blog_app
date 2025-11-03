using Application.Helpers;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Application.Extensions;
/// <summary>
///  Contains all behavior extension is static method
/// </summary>
internal static partial class StringExtensions
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
    /// <summary>
    ///  Get all name arguments in template
    /// </summary>
    /// <param name="template">template</param>
    /// <returns>
    ///     Return all arguments interpolation in template and it is distinct value not compare with lower and upper case
    /// </returns>
    public static string[] GetAllArgumentsInterpolateInTemplate(this string template)
    {
        List<string> vars = [];
        var regex = InterpolateStringRegex();
        foreach (Match match in regex.Matches(template))
        {
            vars.Add(match.Groups[1].Value);
        }

        return [.. vars];
    }
    /// <summary>
    ///  Add param into string interpolate 
    /// </summary>
    /// <param name="interpolate">string interpolate</param>
    /// <param name="param">param to fill into string</param>
    /// <returns>
    ///     Return string after fill all param into interpolate string
    /// </returns>
    public static string InterpolateStringWithDictionaryParam(this string interpolate, Dictionary<string, object?> param)
    {
        string result = InterpolateStringRegex().Replace(interpolate, match =>
        {
            string key = match.Groups[1].Value;
            return param.TryGetValue(key, out var val) ? val?.ToString() ?? string.Empty : match.Value;
        });
        return result;
    }

    [GeneratedRegex(@"\{\{(\w+)\}\}")]
    private static partial Regex InterpolateStringRegex();
}
