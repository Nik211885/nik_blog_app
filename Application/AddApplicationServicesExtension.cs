using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class AddApplicationServicesExtension
{
    /// <summary>
    ///  Add all services in application layer it is services for application
    /// </summary>
    /// <param name="services">services collection</param>
    /// <returns>
    ///     Return services collection after add services in application layer
    /// </returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddServices();
        return services;
    }
}