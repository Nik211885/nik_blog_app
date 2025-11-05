using System.Security.Claims;

namespace Application.Adapters;

public interface IJwtManagement
{
    /// <summary>
    ///     Generator token with claims is username and other claims
    /// </summary>
    /// <param name="userName">Username for user</param>
    /// <param name="claims">Claim for user</param>
    /// <returns>
    ///     Return jwt data model include access token and refresh token
    /// </returns>
    Task<JwtResult> GenerateTokensAsync(string userName, List<Claim> claims);
    /// <summary>
    ///     Provider new access token when token has expriration time
    /// </summary>
    /// <param name="refreshToken">refresh token need to provider new access token</param>
    /// <param name="accessToken">access token has expriration time</param>
    /// <returns>
    ///     Return new jwt data model include access token and refresh token
    /// </returns>
    Task<JwtResult> RefreshTokenAsync(string refreshToken, string accessToken);
    /// <summary>
    ///     Remove refresh token in server when user logout
    /// </summary>
    /// <param name="userName">username is logout</param>
    Task RemoveRefreshTokenByUserNameAsync(string userName);
}


public record JwtResult(
    string AccessToken,
    string RefreshToken,
    int ExpirationAccessToken,
    int ExpirationRefreshToken);