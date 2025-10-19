using System.Security.Claims;
using System.Security.Principal;
using Application.Adapters;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Adapters;

public class UserProvider : IUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    ///  Get user id
    /// </summary>
    public Guid? UserId {
        get
        {
            if (Guid.TryParse(_httpContextAccessor.HttpContext
                    .User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                    out var userId))
            {
                return userId;
            }
            return null;
        } 
    } 
    /// <summary>
    ///  Get username
    /// </summary>
    public string? Username {
        get
        {
            return _httpContextAccessor.HttpContext?.User?.Claims
                .FirstOrDefault(c=>c.Type == ClaimTypes.Name)?.Value;
        } 
    } 
    /// <summary>
    ///     Check token has valid
    /// </summary>
    public bool IsAuthenticated {
        get
        {
            IIdentity? identity = _httpContextAccessor.HttpContext.User.Identity;
            return identity is not null && identity.IsAuthenticated;  
        } 
    }
}