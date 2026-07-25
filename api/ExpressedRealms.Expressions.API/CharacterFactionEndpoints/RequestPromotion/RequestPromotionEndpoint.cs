using ExpressedRealms.Expressions.API.CharacterFactionEndpoints.LeaveFaction;
using ExpressedRealms.Expressions.UseCases.CharacterFactionMappings.RequestPromotion;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Expressions.API.CharacterFactionEndpoints.RequestPromotion;

public static class RequestPromotionEndpoint
{
    public static async Task<Results<Ok<int>, NotFound, ValidationProblem>> ExecuteAsync(
        int characterId,
        [FromBody] RequestPromotionRequest request,
        [FromServices] IRequestPromotionUseCase leaveFactionUseCase
    )
    {
        var results = await leaveFactionUseCase.ExecuteAsync(
            new ()
            {
                CharacterId = characterId, 
                FactionLevelId = request.FactionLevelId, 
                RequestReason = request.RequestReason
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
