namespace WebApi.Extensions;

public static class HttpContextExtensions
{
    /// <summary>
    ///     Get token in header request with attribute is authorization and default schema is bearer
    /// </summary>
    /// <param name="context">httpContext</param>
    /// <returns>
    ///     Return access token if in header exits authorization and schema is bearer token
    /// </returns>
    public static string? GetAccessToken(this HttpContext context)
    {
        const string bearerPrefix = "Bearer ";
        string? bearerToken = context.Request.Headers["Authorization"].ToString();
        if (string.IsNullOrWhiteSpace(bearerToken))
        {
            return null;
        }
        if (bearerToken.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase))
        {
            return bearerToken.Substring(bearerPrefix.Length).Trim();
        }
        return null;
    }
    /// <summary>
    ///     Get refresh token in http request header with attribute is RefreshToken
    /// </summary>
    /// <param name="context">httpContext</param>
    /// <returns>
    ///     Return refresh token find in http request header with key is RefreshToken otherwise is null value
    /// </returns>
    public static string? GetRefreshToken(this HttpContext context)
    {
        string refreshToken = context.Request.Headers["RefreshToken"].ToString().Trim();
        return refreshToken;
    }
}
