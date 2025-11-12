using ExpressedRealms.Characters.UseCases.AssignedXp.Delete;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Characters.API.AssignedXp.Delete;

public static class DeleteEndpoint
{
    public static async Task<Results<Ok, NotFound, ValidationProblem>> ExecuteAsync(
        int characterId,
        int mappingId,
        [FromServices] IDeleteAssignedXpMappingUseCase createKnowledgeUseCase
    )
    {
        var results = await createKnowledgeUseCase.ExecuteAsync(
            new DeleteAssignedXpMappingModel() { Id = mappingId, CharacterId = characterId }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok();
    }
}
