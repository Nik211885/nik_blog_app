using Application.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Repositories;

internal static  class AddRepositoryExtension
{
    /// <summary>
    ///  Add all repositories to di container
    /// </summary>
    /// <param name="services">services collection</param>
    /// <returns>services collection after add repositories services</returns>
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}