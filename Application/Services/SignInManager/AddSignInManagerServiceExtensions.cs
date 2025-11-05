using Microsoft.Extensions.DependencyInjection;

namespace Application.Services.SignInManager;

internal static class AddSignInManagerServiceExtensions
{
    public static IServiceCollection AddSignInManager(this IServiceCollection services, Action<SignInConfig>? configure = null)
    {
        var config = new SignInConfig();
        configure?.Invoke(config);
        services.AddSingleton(config);
        services.AddScoped<SignInManagerServices>(); 
        return services;
    }
}

public class SignInConfig
{
    public int LoginFailToLockAccount { get; set; } = 5;
    public int MinuteLockAccountWhenLoginFail { get; set; } = 1;
}
