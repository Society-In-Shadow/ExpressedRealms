using ExpressedRealms.Authentication.AzureKeyVault;
using ExpressedRealms.Authentication.AzureKeyVault.Secrets;
using ExpressedRealms.FeatureFlags.FeatureClient;
using ExpressedRealms.FeatureFlags.FeatureManager;
using Microsoft.Extensions.DependencyInjection;
using OpenFeature;
using OpenFeature.Contrib.Providers.Flipt;

namespace ExpressedRealms.FeatureFlags.Configuration;

public static class FeatureFlagInjections
{
    public static async Task AddFeatureFlagInjections(
        this IServiceCollection services,
        EarlyKeyVaultManager vaultManager
    )
    {
        var url = await vaultManager.GetSecret(FeatureFlagSettings.FeatureFlagUrl);

        services.AddOpenFeature(featureBuilder =>
        {
            featureBuilder
                .AddHostedFeatureLifecycle()
                .AddProvider(x =>
                {
                    var provider = new FliptProvider(url);
                    return provider;
                });
        });

        services.AddScoped<IFeatureToggleClient, FeatureToggleClient>();
        services.AddScoped<IFeatureToggleManager, FeatureToggleManager>();
    }
}
