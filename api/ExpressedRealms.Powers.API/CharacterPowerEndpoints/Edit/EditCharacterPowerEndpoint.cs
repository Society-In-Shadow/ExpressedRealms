using ExpressedRealms.Knowledges.API.CharacterKnowledges.Edit;
using ExpressedRealms.Powers.UseCases.CharacterPower.Edit;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Powers.API.CharacterPowerEndpoints.Edit;

public static class EditCharacterPowerEndpoint
{
    public static async Task<Results<Ok, NotFound, ValidationProblem>> EditKnowledges(
        int characterId,
        int powerId,
        [FromBody] EditCharacterPowerRequest request,
        [FromServices] IUpdatePowerForCharacterUseCase editKnowledgeUseCase
    )
    {
        var results = await editKnowledgeUseCase.ExecuteAsync(
            new()
            {
                PowerId = powerId,
                CharacterId = characterId,
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
