using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OpenFeature;
using OpenFeature.Model;

namespace ExpressedRealms.FeatureFlags.FeatureClient;

internal sealed class FeatureToggleClient(
    ILogger<FeatureToggleClient> logger,
    IFeatureClient client,
    IHttpContextAccessor httpContextAccessor
) : IFeatureToggleClient
{
    public async Task<bool> HasFeatureFlag(ReleaseFlags releaseName)
    {
        var context = EvaluationContext
            .Builder()
            .SetTargetingKey(httpContextAccessor.HttpContext?.User.Identity?.Name ?? "")
            .Build();

        var value = await client.GetBooleanValueAsync(releaseName.Value, false, context);

        logger.LogInformation(
            "Feature Flag \"{flagName}\" is \"{status}\"",
            releaseName.Name,
            value ? "Enabled" : "Disabled"
        );
        return value;
    }
}
