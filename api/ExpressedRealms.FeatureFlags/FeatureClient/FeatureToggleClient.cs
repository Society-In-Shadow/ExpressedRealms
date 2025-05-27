using Microsoft.Extensions.Logging;
using OpenFeature;
using OpenFeature.Model;

namespace ExpressedRealms.FeatureFlags.FeatureClient;

internal sealed class FeatureToggleClient(ILogger<FeatureToggleClient> logger, IFeatureClient client) : IFeatureToggleClient
{
    public async Task<bool> HasFeatureFlag(ReleaseFlags releaseName)
    {
        var context = EvaluationContext.Builder()
            .SetTargetingKey("cam")
            .Set("extra-data-1", "extra-data-1-value")
            .Build();
        
        var value = await client.GetBooleanValueAsync(releaseName.Value, false, context);
        
        logger.LogInformation("Feature Flag \"{flagName}\" is \"{status}\"", releaseName.Name, value ? "Enabled" : "Disabled");
        return value;
    }
}