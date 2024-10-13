using ExpressedRealms.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace ExpressedRealms.Server.Extensions;

public static class PolicyExtensions
{
    public static async Task<bool> UserHasPolicyAsync(this HttpContext httpContext, Policies policy)
    {
        // Resolve IAuthorizationService from HttpContext
        var authorizationService =
            httpContext.RequestServices.GetRequiredService<IAuthorizationService>();

        // Perform the policy check
        var result = await authorizationService.AuthorizeAsync(httpContext.User, policy.Name);
        return result.Succeeded;
    }

    public static TBuilder RequirePolicyAuthorization<TBuilder>(
        this TBuilder builder,
        Policies policy
    )
        where TBuilder : IEndpointConventionBuilder
    {
        if (builder == null)
            throw new ArgumentNullException(nameof(builder));
        ArgumentNullException.ThrowIfNull(policy, nameof(policy));
        return builder.RequireAuthorization(policy.Name);
    }
}
