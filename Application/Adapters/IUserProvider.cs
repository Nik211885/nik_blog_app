namespace Application.Adapters;
/// <summary>
///  Get information in body token 
/// </summary>
public interface IUserProvider
{
    /// <summary>
    ///  User id in body token
    /// </summary>
    public Guid UserId { get; }
    /// <summary>
    ///  User name in token
    /// </summary>
    public string Username { get; }
    /// <summary>
    ///  Check token has valid
    /// </summary>
    bool IsAuthenticated { get; }
}