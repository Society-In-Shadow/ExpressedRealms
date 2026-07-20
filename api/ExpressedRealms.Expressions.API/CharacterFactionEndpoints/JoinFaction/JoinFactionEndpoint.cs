using ExpressedRealms.Expressions.UseCases.CharacterFactionMapping.JoinFaction;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Expressions.API.CharacterFactionEndpoints.JoinFaction;

public static class JoinFactionEndpoint
{
    public static async Task<Results<Ok<int>, NotFound, ValidationProblem>> ExecuteAsync(
        int characterId,
        int factionId,
        [FromServices] IJoinFactionUseCase joinFactionUseCase
    )
    {
        var results = await joinFactionUseCase.ExecuteAsync(
            new JoinFactionModel()
            {
                CharacterId = characterId,
                FactionId = factionId,
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
