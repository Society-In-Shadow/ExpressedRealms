using ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.Delete;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Knowledges.API.CharacterKnowledges.Delete;

public static class DeleteCharacterKnowledgeEndpoint
{
    public static async Task<Results<Ok, NotFound, ValidationProblem>> DeleteMapping(
        int characterId,
        int mappingId,
        [FromServices] IDeleteKnowledgeFromCharacterUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new DeleteKnowledgeFromCharacterModel() { MappingId = mappingId }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok();
    }
}
