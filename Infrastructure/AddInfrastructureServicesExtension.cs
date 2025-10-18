using Infrastructure.Configurations;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class AddInfrastructureServicesExtension
{
    /// <summary>
    ///  Add all services in infrastructure layer it is includes repository all external
    /// services ...,
    /// </summary>
    /// <param name="services">services collection</param>
    /// <param name="configuration"></param>
    /// <returns>
    ///     Return services collection after add services in infrastructure layer
    /// </returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddConfigurations(configuration);
        services.AddRepositories();
        services.AddApplicationDbContext();
        return services;
    }
}