using Application.Entities;
using Application.Enums;
using Application.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class LoginProviderRepository : RepositoryBase<LoginProvider>, ILoginProviderRepository
{
    private readonly ApplicationDbContext _context;
    public LoginProviderRepository(ApplicationDbContext context)
    : base(context)
    {
        _context = context;
    }
    /// <summary>
    ///     Find provider specific with user id
    /// </summary>
    /// <param name="loginProvider">provider</param>
    /// <param name="identifier">user id</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns>
    ///     Return login provider if match with login provider and user id otherwise is null value
    /// </returns>
    public async Task<LoginProvider?> GetByProviderAsync(LoginProviderEx loginProvider, string identifier, CancellationToken cancellationToken = default)
    {
        LoginProvider? loginProviderFind = await _context.LoginProviders
                        .Where(x => x.Provider == loginProvider && x.Identifier == identifier)
                        .FirstOrDefaultAsync(cancellationToken);
        return loginProviderFind;
    }
}
