using ExpressedRealms.Authentication.PermissionCollection.Support;
using Microsoft.AspNetCore.Builder;

namespace ExpressedRealms.Authentication.PermissionCollection.Configuration;

public static class EndpointExtensions
{
    public static TBuilder RequirePermission<TBuilder>(
        this TBuilder builder,
        IPermissionAction permission)
        where TBuilder : IEndpointConventionBuilder
    {
        return builder.RequireAuthorization(policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.AddRequirements(new PermissionRequirement(permission));
        });
    }
}