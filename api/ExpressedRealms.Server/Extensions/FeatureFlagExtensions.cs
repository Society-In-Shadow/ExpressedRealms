using ExpressedRealms.FeatureFlags;
using ExpressedRealms.FeatureFlags.FeatureClient;

namespace ExpressedRealms.Server.Extensions;

public static class FeatureFlagExtensions
{
    // Define a feature toggle filter that checks if a user has a specific feature enabled
    public static RouteHandlerBuilder RequireFeatureToggle(
        this RouteHandlerBuilder builder,
        ReleaseFlags releaseFlag
    )
    {
        return builder.AddEndpointFilter(
            async (context, next) =>
            {
                var httpContext = context.HttpContext;

                var featureToggleClient =
                    httpContext.RequestServices.GetRequiredService<IFeatureToggleClient>();
                // Replace this with your actual logic to check feature toggle for user
                var hasFeature = await featureToggleClient.HasFeatureFlag(releaseFlag);

                if (!hasFeature)
                {
                    // Return 403 Forbidden if feature is not enabled
                    return TypedResults.StatusCode(StatusCodes.Status403Forbidden);
                }

                // Continue to the next delegate in the pipeline
                return await next(context);
            }
        );
    }

    public static RouteGroupBuilder RequireFeatureToggle(
        this RouteGroupBuilder builder,
        ReleaseFlags releaseFlag
    )
    {
        return builder.AddEndpointFilter(
            async (context, next) =>
            {
                var httpContext = context.HttpContext;

                var featureToggleClient =
                    httpContext.RequestServices.GetRequiredService<IFeatureToggleClient>();
                // Replace this with your actual logic to check feature toggle for user
                var hasFeature = await featureToggleClient.HasFeatureFlag(releaseFlag);

                if (!hasFeature)
                {
                    // Return 403 Forbidden if feature is not enabled
                    return TypedResults.StatusCode(StatusCodes.Status403Forbidden);
                }

                // Continue to the next delegate in the pipeline
                return await next(context);
            }
        );
    }
}
