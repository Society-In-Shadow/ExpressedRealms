using ExpressedRealms.Characters.UseCases.Characters.GetBreakOfDawnInfo;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Characters.API.CharacterEndPoints.GetBreakOfDawnInfo;

public static class GetBreakOfDawnInfoEndpoint
{
    public static async Task<
        Results<Ok<GetBreakOfDawnInfoResponse>, ValidationProblem, NotFound>
    > ExecuteAsync(int characterId, [FromServices] IGetBreakOfDawnInfoUseCase useCase)
    {
        var results = await useCase.ExecuteAsync(new() { Id = characterId });

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(
            new GetBreakOfDawnInfoResponse()
            {
                Health = results.Value.Health,
                Vitality = results.Value.Vitality,
                Psyche = results.Value.Psyche,
                Blood = results.Value.Blood,
                Rwp = results.Value.Rwp,
                Mortis = results.Value.Mortis,
                CharacterLevel = results.Value.CharacterLevel,
                ExpressionId = results.Value.ExpressionId
            }
        );
    }
}
