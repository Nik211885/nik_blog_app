using Infrastructure.Adapters;
using Infrastructure.Data;
using Infrastructure.Extend;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class AddInfrastructureServicesExtension
{
    /// <summary>
    ///  Add all services in infrastructure layer it is includes repository all external
    /// services ...,
    /// </summary>
    /// <param name="services">services collection</param>
    /// <returns>
    ///     Return services collection after add services in infrastructure layer
    /// </returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddRepositories();
        services.AddApplicationDbContext();
        services.AddAdapterServices();
        services.AddExtendServices();
        return services;
    }
}