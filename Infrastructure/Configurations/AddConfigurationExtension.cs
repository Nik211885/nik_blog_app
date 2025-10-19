using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configurations;

internal static class AddConfigurationExtension
{
    /// <summary>
    ///  Scan all class model and add to option to services container
    /// </summary>
    /// <param name="services">services collection</param>
    /// <param name="configuration"></param>
    /// <returns>
    ///     Return services collection after scan and add option to container
    /// </returns>
    internal static IServiceCollection AddConfigurations(this IServiceCollection services,
        IConfiguration  configuration)
    {
        var postgresConnection = "PostgresConnectionString";
        var jwtAuthentication = "JwtAuthentication";
        
        services.Configure<PostgresConnectionString>(configuration.GetSection(postgresConnection));
        services.Configure<JwtConfigurationDataModel>(configuration.GetSection(jwtAuthentication));
        return services;
    }
}