using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.Expressions.Repository.ExpressionTextSections;
using ExpressedRealms.FeatureFlags;
using ExpressedRealms.FeatureFlags.FeatureClient;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Expressions.API.ExpressionEndpoints.GetExpressionId;

internal static class GetExpressionIdByNameEndpoint
{
    public static async Task<Results<NotFound, Ok<ExpressionNameResponse>>> GetExpressionIdByName(
        string name,
        IExpressionTextSectionRepository sectionRepository,
        IExpressionRepository expressionRepository,
        IFeatureToggleClient featureToggleClient
    )
    {
        var expressionIdResult = await sectionRepository.GetExpressionId(name);
        if (expressionIdResult.HasNotFound(out var notFound))
            return notFound;
        expressionIdResult.ThrowIfErrorNotHandled();
        var expressionId = expressionIdResult.Value;

        return TypedResults.Ok(
            new ExpressionNameResponse
            {
                Id = expressionId,
                ShowPowersTab = await featureToggleClient.HasFeatureFlag(
                    ReleaseFlags.ShowPowersTab
                ),
            }
        );
    }
}
