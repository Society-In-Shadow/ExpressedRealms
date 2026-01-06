using ExpressedRealms.FeatureFlags.FeatureClient;
using ExpressedRealms.FeatureFlags.FeatureManager;
using ExpressedRealms.Shared.AzureKeyVault;
using ExpressedRealms.Shared.AzureKeyVault.Secrets;
using Microsoft.Extensions.DependencyInjection;
using OpenFeature;
using OpenFeature.Contrib.Providers.Flipt;

namespace ExpressedRealms.FeatureFlags.Configuration;

public static class FeatureFlagInjections
{
    public static void AddFeatureFlagInjections(this IServiceCollection services)
    {
        var url = KeyVaultManager.GetSecret(FeatureFlagSettings.FeatureFlagUrl);

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
