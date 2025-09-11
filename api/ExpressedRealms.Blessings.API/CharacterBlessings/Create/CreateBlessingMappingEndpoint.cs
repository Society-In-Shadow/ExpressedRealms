using ExpressedRealms.Blessings.UseCases.CharacterBlessingMappings.Create;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Blessings.API.CharacterBlessings.Create;

public static class CreateBlessingMappingEndpoint
{
    public static async Task<
        Results<Ok<int>, NotFound, ValidationProblem, BadRequest<string>>
    > ExecuteAsync(
        int characterId,
        [FromBody] CreateBlessingMappingRequest request,
        [FromServices] IAddBlessingToCharacterUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new AddBlessingToCharacterModel()
            {
                CharacterId = characterId,
                BlessingLevelId = request.BlessingLevelId,
                BlessingId = request.BlessingId,
                Notes = request.Notes,
            }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasInsufficientXP(out var insufficientXp))
            return insufficientXp;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(results.Value);
    }
}
