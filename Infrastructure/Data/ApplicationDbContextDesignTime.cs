using Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Infrastructure.Data;

public class ApplicationDbContextDesignTime 
    : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        string basePath = Path.Combine(Directory.GetCurrentDirectory(), "../WebApi");
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();
        PostgresConnectionString? postgresConnection = configuration.GetSection("PostgresConnectionString")
            .Get<PostgresConnectionString>();
        ArgumentNullException.ThrowIfNull(postgresConnection);
        IOptions<PostgresConnectionString> options = new OptionsWrapper<PostgresConnectionString>(postgresConnection);
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql(postgresConnection.Default);
        return new ApplicationDbContext(optionsBuilder.Options, options);
    }
}