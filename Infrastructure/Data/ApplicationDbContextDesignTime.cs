using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

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
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        return new ApplicationDbContext(optionsBuilder.Options, configuration);
    }
}