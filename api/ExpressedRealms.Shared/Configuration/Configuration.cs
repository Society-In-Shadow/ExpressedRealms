using ExpressedRealms.Shared.AzureKeyVault;
using Microsoft.Extensions.DependencyInjection;

namespace ExpressedRealms.Shared.Configuration;

public static class SharedInjections
{
    public static IServiceCollection AddAuthenticationInjections(this IServiceCollection services)
    {
        services.AddSingleton<IKeyVaultManager, KeyVaultManager>();
        return services;
    }
}