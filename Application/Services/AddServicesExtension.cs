using Application.Services.UserManager;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services;

internal static class AddServicesExtension 
{
    /// <summary>
    ///  Add all services for di container
    /// </summary>
    /// <param name="services">services collection</param>
    /// <returns>Services collection</returns>
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<UserManagerServices>();
        return services;
    }
}