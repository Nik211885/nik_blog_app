using Application.Adapters;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Adapters;

internal static class AddAdapterServicesExtension
{
    /// <summary>
    ///     Add adapter services 
    /// </summary>
    /// <param name="services">services collection</param>
    /// <returns>services collection</returns>
    internal static IServiceCollection AddAdapterServices(this IServiceCollection services)
    {
        services.AddSingleton<IJwtManagement, JwtManagement>();
        services.AddScoped<IUserProvider, UserProvider>();
        return services;
    }
}