using ExpressedRealms.Shared.AzureKeyVault;
using Microsoft.Extensions.DependencyInjection;

namespace ExpressedRealms.Shared.Configuration;

public static class SharedInjections
{
    public static IServiceCollection AddSharedInjections(this IServiceCollection services)
    {
        services.AddSingleton<IKeyVaultManager, KeyVaultManager>();
        return services;
    }
}