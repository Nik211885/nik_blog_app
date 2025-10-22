using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data;

internal static class AddDbContextServicesExtension
{
    /// <summary>
    ///  Add application db content add migration db schema for database
    ///  or seeding some data
    /// </summary>
    /// <param name="services">services collection</param>
    /// <returns>
    ///     Return services collection after add application db context
    /// </returns>
    internal static IServiceCollection AddApplicationDbContext(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>();
        return services;
    }
}