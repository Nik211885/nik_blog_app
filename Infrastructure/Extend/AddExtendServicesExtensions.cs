using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extend;


internal static class AddExtendServicesExtensions
{
    /// <summary>
    ///     Add all extend services just is all presentation want 
    /// </summary>
    /// <param name="services">services collection</param>
    /// <returns>services collection after add extend services</returns>
    public static IServiceCollection AddExtendServices(this IServiceCollection services)
    {
        services.AddSingleton<CloudinaryServices>();
        return services;
    }
}