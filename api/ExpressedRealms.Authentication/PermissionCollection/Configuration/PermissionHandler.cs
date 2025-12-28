using Microsoft.AspNetCore.Authorization;

namespace ExpressedRealms.Authentication.PermissionCollection.Configuration;

public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement
    )
    {
        if (context.User.HasClaim("permission", requirement.Permission.Key))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
