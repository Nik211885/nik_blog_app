using Microsoft.EntityFrameworkCore;
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
        services.MigrationDatabase();
        return services;
    }
    /// <summary>
    ///     Migration database
    /// </summary>
    /// <param name="services">services collection will build for services provider</param>
    private static void MigrationDatabase(this IServiceCollection services)
    {
        IServiceProvider serviceProvider = services.BuildServiceProvider();
        using IServiceScope scope = serviceProvider.CreateScope();
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        if (dbContext.Database.EnsureCreated())
        {
            dbContext.Database.Migrate();
        }
    }
}