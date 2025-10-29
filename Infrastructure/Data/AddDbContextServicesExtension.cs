using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Data;

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
        services.AddDbContext<ApplicationDbContext>((sp, dbOptions) =>
        {
            string connectionString = sp.GetApplicationConnectionString();
            dbOptions.UseNpgsql(connectionString);
        });
        services.AddScoped<IDbConnection>(sp =>
        {
            string connectionString = sp.GetApplicationConnectionString();
            var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            return connection;
        });
        services.MigrationDatabase();
        return services;
    }
    /// <summary>
    ///     Migration database
    /// </summary>
    /// <param name="services">services collection will build for services provider</param>
    private static void MigrationDatabase(this IServiceCollection services)
    {
        ApplicationDbContext dbContext = services.GetApplicatioDbContextFromDiContaine();
        if (dbContext.Database.EnsureCreated())
        {
            dbContext.Database.Migrate();
        }
    }
    /// <summary>
    ///  Get application db context from di container with scope
    /// </summary>
    /// <param name="services">services collection</param>
    /// <returns>
    ///     Return application db context after get in di container
    /// </returns>
    private static ApplicationDbContext GetApplicatioDbContextFromDiContaine(this IServiceCollection services)
    {
        IServiceProvider serviceProvider = services.BuildServiceProvider();
        using IServiceScope scope = serviceProvider.CreateScope();
        ApplicationDbContext applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        return applicationDbContext;
    }
    /// <summary>
    ///  Get connection string form config in presentation with configurations services
    /// </summary>
    /// <returns>
    ///     Return connect string if find otherwitse throw exception arguments is null
    /// </returns>
    private static string GetApplicationConnectionString(this IServiceProvider sp)
    {
        IConfiguration configuration = sp.GetRequiredService<IConfiguration>();
        string? connectionString = configuration.GetSection("PostgresConnectionString:Default").Get<string>();
        ArgumentNullException.ThrowIfNullOrWhiteSpace(connectionString);
        return connectionString;
    }
}