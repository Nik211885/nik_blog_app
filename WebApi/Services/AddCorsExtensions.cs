using Microsoft.Identity.Client.Http.Retry;

namespace WebApi.Services;

internal static class AddCorsExtensions
{
    /// <summary>
    ///  Add cors policy services 
    /// </summary>
    /// <param name="services">services collection</param>
    /// <param name="configuration">configuration</param>
    /// <param name="corsName">name cors</param>
    /// <returns>
    ///     Return services collection after add cors policy
    /// </returns>
    public static IServiceCollection AddCorsForRegisterApp(this IServiceCollection services, IConfiguration configuration, string corsName)
    {
        services.AddCors(options =>
        {
            string? clientAppSection = configuration.GetSection("FontEnd").Get<string>();
            List<string> allowedOrigins = [];
            if (clientAppSection is not null)
            {
                allowedOrigins.Add(clientAppSection);
            }
            options.AddPolicy(corsName,
                policy => policy
                    .WithOrigins(allowedOrigins.ToArray()) 
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()); 
        });
        return services;
    }
}