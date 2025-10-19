namespace Infrastructure.Configurations;
/// <summary>
///  Model contains jwt configuration
/// </summary>
public class JwtConfigurationDataModel
{
    /// <summary>
    ///     Url identity provider should application has trust
    ///     application know get public key and config, metadata to valid token
    /// </summary>
    public string Authority { get; set; }
    /// <summary>
    ///     Key to signature payload jwt
    /// </summary>
    public string Secret { get; set; }
    /// <summary>
    ///      Claim inside payload is identifier token has published where
    /// </summary>
    public string Issuer { get; set; }
    /// <summary>
    ///     Claim inside payload is identifier token will provider what application
    /// </summary>
    public string Audience { get; set; }
    /// <summary>
    ///     Time stamp for expiration access token
    /// </summary>
    public int AccessTokenExpiration { get; set; }
    /// <summary>
    ///     Time stamp for expiration refresh token
    /// </summary>
    public int RefreshTokenExpiration { get; set; }
}