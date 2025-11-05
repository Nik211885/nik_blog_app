using Application.Adapters;
using Application.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Repositories;

internal static class AddRepositoryExtension
{
    /// <summary>
    ///  Add all repositories to di container
    /// </summary>
    /// <param name="services">services collection</param>
    /// <returns>services collection after add repositories services</returns>
    internal static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<INotificationTemplateRepository, NotificationTemplateRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IMalInfoRepository, MailInfoRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IArgumentRepository, ArgumentRepository>();
        services.AddScoped<ILoginProviderRepository, LoginProviderRepository>();
        return services;
    }
}