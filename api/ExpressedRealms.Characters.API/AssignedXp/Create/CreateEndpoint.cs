using ExpressedRealms.Characters.UseCases.AssignedXp.Create;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Characters.API.AssignedXp.Create;

public static class CreateEndpoint
{
    public static async Task<Results<Ok<int>, NotFound, ValidationProblem>> ExecuteAsync(
        int characterId,
        [FromBody] CreateRequest request,
        [FromServices] ICreateAssignedXpMappingUseCase createKnowledgeUseCase
    )
    {
        var results = await createKnowledgeUseCase.ExecuteAsync(
            new CreateAssignedXpMappingModel()
            {
                EventId = request.EventId,
                AssignedXpTypeId = request.AssignedXpTypeId,
                Reason = request.Reason,
                CharacterId = characterId,
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
