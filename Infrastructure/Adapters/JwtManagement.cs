using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Adapters;
using Application.Exceptions;
using Infrastructure.Configurations;
using Infrastructure.Helpers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Adapters;

public class JwtManagement : IJwtManagement
{
    private readonly IMemoryCache _cache;
    private readonly byte[] _secret;
    private readonly JwtConfigurationDataModel _jwtConfigurationDataModel;
    private readonly string _prefixRefeshToken = "refresh_token{0}";

    public JwtManagement(IMemoryCache cache, 
        IOptions<JwtConfigurationDataModel> jwtConfigurationDataModelOptions)
    {
        _cache = cache;
        _jwtConfigurationDataModel = jwtConfigurationDataModelOptions.Value;
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
    public JwtResult GenerateTokens(string userName, List<Claim> claims)
    {
        var jwtToken = new JwtSecurityToken(
                issuer: _jwtConfigurationDataModel.Issuer,
                claims: claims,
                audience: _jwtConfigurationDataModel.Audience,
                expires: DateTime.UtcNow.AddMinutes(_jwtConfigurationDataModel.AccessTokenExpiration)
            );
        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        var refreshToken = GenerateRefreshToken();
        string keyStoreRefreshToken = string.Format(_prefixRefeshToken, userName);
        _cache.Set(keyStoreRefreshToken, refreshToken, TimeSpan.FromMinutes(_jwtConfigurationDataModel.RefreshTokenExpiration));
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
    public JwtResult RefreshToken(string refreshToken, string accessToken)
    {
        var (principal, jwtToken) = DecodeAccessToken(accessToken);
        if (jwtToken is null || jwtToken.Header.Alg != SecurityAlgorithms.HmacSha256)
        {
            ThrowHelper.ThrowUnauthorized("Invalid access token");
        }

        string? userName = principal.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
        if (userName is null)
        {
            ThrowHelper.ThrowUnauthorized("Invalid access token");
        }
        var keyStoreRefreshToken = string.Format(_prefixRefeshToken, userName);
        if (!_cache.TryGetValue(keyStoreRefreshToken, out var refresh))
        {
            ThrowHelper.ThrowUnauthorized("Invalid refresh token");
        }
        if (refresh is null || !refresh.Equals(refreshToken))
        {
            ThrowHelper.ThrowUnauthorized("Invalid refresh token");
        }
        return GenerateTokens(userName, principal.Claims.ToList());
    }

    public void RemoveRefreshTokenByUserName(string userName)
    {
        string keyStoreRefreshToken = string.Format(_prefixRefeshToken, userName);
        _cache.Remove(keyStoreRefreshToken);
    }
    /// <summary>
    ///  Validate access token skip to expiration time in toke  
    /// </summary>
    /// <param name="token">access token need valid</param>
    /// <returns>
    ///     Return claims principal and jwt security in token
    /// </returns>
    private (ClaimsPrincipal principal, JwtSecurityToken?)  DecodeAccessToken(string token)
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