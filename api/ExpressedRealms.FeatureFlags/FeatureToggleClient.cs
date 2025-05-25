using Microsoft.Extensions.Logging;
using OpenFeature;
using OpenFeature.Model;

namespace ExpressedRealms.FeatureFlags;

internal sealed class FeatureToggleClient(ILogger<FeatureToggleClient> logger, IFeatureClient client) : IFeatureToggleClient
{
    public async Task<bool> HasFeatureFlag(FeatureFlags featureName)
    {
        var context = EvaluationContext.Builder()
            .SetTargetingKey("cam")
            .Set("extra-data-1", "extra-data-1-value")
            .Build();
        
        var value = await client.GetBooleanValueAsync(featureName.Name, false, context);
        
        logger.LogInformation("Flag Flag \"{flagName}\" is \"{status}\"", featureName.Name, value ? "Enabled" : "Disabled");
        return value;
    }
}