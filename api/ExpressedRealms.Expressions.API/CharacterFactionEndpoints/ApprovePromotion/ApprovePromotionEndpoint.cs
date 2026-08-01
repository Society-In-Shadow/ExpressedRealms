using ExpressedRealms.Expressions.API.CharacterFactionEndpoints.LeaveFaction;
using ExpressedRealms.Expressions.UseCases.CharacterFactionMappings.ApprovePromotion;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Expressions.API.CharacterFactionEndpoints.ApprovePromotion;

public static class ApprovePromotionEndpoint
{
    public static async Task<Results<Ok<int>, NotFound, ValidationProblem>> ExecuteAsync(
        int characterId,
        [FromBody] ApprovePromotionRequest request,
        [FromServices] IApprovePromotionUseCase leaveFactionUseCase
    )
    {
        var results = await leaveFactionUseCase.ExecuteAsync(
            new()
            {
                CharacterId = characterId,
                FactionLevelId = request.FactionLevelId,
                ApprovalReason = request.ApprovalReason,
            }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(results.Value);
    }
}
