using ExpressedRealms.Authentication.AzureKeyVault;
using Microsoft.Extensions.DependencyInjection;

namespace ExpressedRealms.Authentication.Configuration;

public static class AuthenticationInjections
{
    public static IServiceCollection AddAuthenticationInjections(this IServiceCollection services)
    {
        services.AddSingleton<IKeyVaultManager, KeyVaultManager>();
        return services;
    }
}
