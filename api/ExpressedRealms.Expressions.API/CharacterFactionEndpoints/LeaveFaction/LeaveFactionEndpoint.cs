using ExpressedRealms.Expressions.UseCases.CharacterFactionMappings.LeaveFaction;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Expressions.API.CharacterFactionEndpoints.LeaveFaction;

public static class LeaveFactionEndpoint
{
    public static async Task<Results<Ok<int>, NotFound, ValidationProblem>> ExecuteAsync(
        int characterId,
        [FromServices] ILeaveFactionUseCase leaveFactionUseCase
    )
    {
        var results = await leaveFactionUseCase.ExecuteAsync(
            new LeaveFactionModel() { CharacterId = characterId }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(results.Value);
    }
}
