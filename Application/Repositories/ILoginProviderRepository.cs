using Application.Entities;
using Application.Enums;

namespace Application.Repositories;

public interface ILoginProviderRepository : IRepositoryBase<LoginProvider>
{
    /// <summary>
    ///     Find provider specific with user id
    /// </summary>
    /// <param name="loginProvider">provider</param>
    /// <param name="identifier">user id</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns>
    ///     Return login provider if match with login provider and user id otherwise is null value
    /// </returns>
    Task<LoginProvider?> GetByProviderAsync(LoginProviderEx loginProvider, string identifier, CancellationToken cancellationToken = default);
}
