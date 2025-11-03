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
        services.AddMemoryCache();
        services.AddSingleton<IJwtManagement, JwtManagement>();
        services.AddScoped<IEmailServices, EmailServices>();
        services.AddScoped<IUserProvider, UserProvider>();
        services.AddScoped<IConnectionQueryService, ConnectionQueryServices>();
        services.AddSingleton<ITokenEncryptionService, TokenEncryptionService>();
        return services;
    }
}