using ExpressedRealms.Blessings.UseCases.CharacterBlessingMappings.Edit;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Blessings.API.CharacterBlessings.Edit;

public static class EditCharacterBlessingEndpoint
{
    public static async Task<Results<Ok, NotFound, ValidationProblem>> ExecuteAsync(
        int characterId,
        int mappingId,
        [FromBody] EditCharacterBlessingRequest request,
        [FromServices] IUpdateBlessingForCharacterUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new UpdateBlessingForCharacterModel()
            {
                MappingId = mappingId,
                CharacterId = characterId,
                BlessingLevelId = request.BlessingLevelId,
                Notes = request.Notes,
            }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok();
    }
}
