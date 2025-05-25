using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using OpenFeature;
using OpenFeature.Contrib.Providers.Flipt;

namespace ExpressedRealms.FeatureFlags.Configuration;

public static class FeatureFlagInjections
{
    public static IServiceCollection AddFeatureFlagInjections(this IServiceCollection services)
    {
        services.AddOpenFeature(featureBuilder =>
        {
            featureBuilder
                .AddHostedFeatureLifecycle() // From Hosting package
                .AddProvider(x =>
                {
                    var provider = new FliptProvider(Environment.GetEnvironmentVariable("FEATURE-FLAG-URL"));
                    return provider;
                });
        });
        
        services.AddScoped<IFeatureToggleClient, FeatureToggleClient>();
        
        return services;
    }
    
    public static IHealthChecksBuilder AddFliptHealthCheck(
        this IHealthChecksBuilder builder,
        string name = "flipt")
    {
        return builder.AddCheck<FliptHealthCheck>(name, failureStatus: HealthStatus.Degraded);
    }

}