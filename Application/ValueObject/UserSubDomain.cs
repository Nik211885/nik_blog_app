using Application.Entities;

namespace Application.ValueObject;

public class UserSubDomain
{
    /// <summary>
    ///  Subdomain for blog with each user
    /// </summary>
    public string? SubDomainBlogSlug { get; set; }
    /// <summary>
    ///  Bio for sub domain
    /// </summary>
    public string? BioDomainBlog { get; set; }
    /// <summary>
    ///  Cout followers subdomain
    /// </summary>
    public long CoutFlowers { get; set; }
    /// <summary>
    ///  lock for subdomain
    /// </summary>
    public LockEntity? LockSubDomain { get; set; }
}