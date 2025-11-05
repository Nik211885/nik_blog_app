using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Adapters;
using Application.Exceptions;
using Application.Helpers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Adapters;

public class JwtManagement : IJwtManagement
{
    private readonly ICache _cache;
    private readonly byte[] _secret;
    private readonly JwtConfigurationDataModel _jwtConfigurationDataModel;
    private readonly string _prefixRefreshToken = "refresh_token{0}";

    public JwtManagement(ICache cache,
        IConfiguration configuration)
    {
        _cache = cache;
        JwtConfigurationDataModel? jwtConfigurationDataModel =
            configuration.GetSection("JwtAuthentication").Get<JwtConfigurationDataModel>();
        ArgumentNullException.ThrowIfNull(jwtConfigurationDataModel);
        _jwtConfigurationDataModel = jwtConfigurationDataModel;
        _secret = Encoding.UTF8.GetBytes(_jwtConfigurationDataModel.Secret);
    }
    /// <summary>
    ///     Generator new token
    /// </summary>
    /// <param name="userName">username for key in refresh token in cache</param>
    /// <param name="claims">claim in token</param>
    /// <returns>
    ///     Return jwt result model include access token and refresh token
    /// </returns>
    public async Task<JwtResult> GenerateTokensAsync(string userName, List<Claim> claims)
    {
        var jwtToken = new JwtSecurityToken(
                issuer: _jwtConfigurationDataModel.Issuer,
                claims: claims,
                audience: _jwtConfigurationDataModel.Audience,
                expires: DateTime.UtcNow.AddMinutes(_jwtConfigurationDataModel.AccessTokenExpiration)
            );
        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        var refreshToken = GenerateRefreshToken();
        string keyStoreRefreshToken = string.Format(_prefixRefreshToken, userName);
        await _cache.SetItem(keyStoreRefreshToken, refreshToken, TimeSpan.FromMinutes(_jwtConfigurationDataModel.RefreshTokenExpiration));
        return new JwtResult(
                AccessToken: accessToken,
                RefreshToken: refreshToken,
                ExpirationAccessToken: _jwtConfigurationDataModel.AccessTokenExpiration,
                ExpirationRefreshToken: _jwtConfigurationDataModel.RefreshTokenExpiration
            );
    }
    /// <summary>
    ///     Provider new token when access token has expiration time
    /// </summary>
    /// <param name="refreshToken">refresh token need get new token</param>
    /// <param name="accessToken">old access token</param>
    /// <returns>
    ///     Return new jwt token data model
    /// </returns>
    public async Task<JwtResult> RefreshTokenAsync(string refreshToken, string accessToken)
    {
        var (principal, jwtToken) = DecodeAccessToken(accessToken);
        if (jwtToken is null || jwtToken.Header.Alg != SecurityAlgorithms.HmacSha256)
        {
            ThrowHelper.ThrowWhenUnauthorized("Invalid access token");
        }

        string? userName = principal.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
        if (userName is null)
        {
            ThrowHelper.ThrowWhenUnauthorized("Invalid access token");
        }
        var keyStoreRefreshToken = string.Format(_prefixRefreshToken, userName);
        if (!await _cache.TryGetItem(keyStoreRefreshToken, out var refresh))
        {
            ThrowHelper.ThrowWhenUnauthorized("Invalid refresh token");
        }
        if (refresh is null || !refresh.Equals(refreshToken))
        {
            ThrowHelper.ThrowWhenUnauthorized("Invalid refresh token");
        }
        var result = await GenerateTokensAsync(userName, [.. principal.Claims]);

        return result;
    }

    public async Task RemoveRefreshTokenByUserNameAsync(string userName)
    {
        string keyStoreRefreshToken = string.Format(_prefixRefreshToken, userName);
        await _cache.RemoveItem(keyStoreRefreshToken);
    }
    /// <summary>
    ///  Validate access token skip to expiration time in toke  
    /// </summary>
    /// <param name="token">access token need valid</param>
    /// <returns>
    ///     Return claims principal and jwt security in token
    /// </returns>
    private (ClaimsPrincipal principal, JwtSecurityToken?) DecodeAccessToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new ArgumentException("Invalid token");
        }
        var principal = new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _jwtConfigurationDataModel.Issuer,
            ValidateAudience = true,
            ValidAudience = _jwtConfigurationDataModel.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(_secret),
            ValidateLifetime = false,
            ClockSkew = TimeSpan.Zero
        }, out var validatedToken);
        return (principal, validatedToken as JwtSecurityToken);
    }
    /// <summary>
    ///  Factory new refresh token
    /// </summary>
    /// <returns></returns>
    private static string GenerateRefreshToken()
    {
        return StringHelper.GeneratorRandomStringBase64(32);
    }
}




/// <summary>
///  Model contains jwt configuration
/// </summary>
internal class JwtConfigurationDataModel
{
    /// <summary>
    ///     Url identity provider should application has trust
    ///     application know get public key and config, metadata to valid token
    /// </summary>
    public string Authority { get; set; } = string.Empty;
    /// <summary>
    ///     Key to signature payload jwt
    /// </summary>
    public string Secret { get; set; } = string.Empty;
    /// <summary>
    ///      Claim inside payload is identifier token has published where
    /// </summary>
    public string Issuer { get; set; } = string.Empty;
    /// <summary>
    ///     Claim inside payload is identifier token will provider what application
    /// </summary>
    public string Audience { get; set; } = string.Empty;
    /// <summary>
    ///     Time stamp for expiration access token
    /// </summary>
    public int AccessTokenExpiration { get; set; }
    /// <summary>
    ///     Time stamp for expiration refresh token
    /// </summary>
    public int RefreshTokenExpiration { get; set; }
}