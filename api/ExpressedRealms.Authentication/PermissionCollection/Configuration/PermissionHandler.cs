using ExpressedRealms.FeatureFlags;
using ExpressedRealms.FeatureFlags.FeatureClient;
using Microsoft.AspNetCore.Authorization;

namespace ExpressedRealms.Authentication.PermissionCollection.Configuration;

public class PermissionHandler(IFeatureToggleClient featureToggleClient) : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement
    )
    {
        if (! await featureToggleClient.HasFeatureFlag(ReleaseFlags.UseNewPermissionSystem))
        {
            context.Succeed(requirement);
            return;
        }

        if (context.User.HasClaim("permission", requirement.Permission.Key))
        {
            context.Succeed(requirement);
        }
    }
}
