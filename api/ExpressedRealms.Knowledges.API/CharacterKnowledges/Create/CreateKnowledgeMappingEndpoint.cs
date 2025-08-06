using ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.Create;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Knowledges.API.CharacterKnowledges.Create;

public static class CreateKnowledgeMappingEndpoint
{
    public static async Task<
        Results<Ok<int>, NotFound, ValidationProblem, BadRequest<string>>
    > CreateMapping(
        int characterId,
        [FromBody] CreateKnowledgeMappingRequest request,
        [FromServices] IAddKnowledgeToCharacterUseCase createKnowledgeUseCase
    )
    {
        var results = await createKnowledgeUseCase.ExecuteAsync(
            new AddKnowledgeToCharacterModel()
            {
                CharacterId = characterId,
                KnowledgeId = request.KnowledgeId,
                KnowledgeLevelId = request.KnowledgeLevelId,
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
