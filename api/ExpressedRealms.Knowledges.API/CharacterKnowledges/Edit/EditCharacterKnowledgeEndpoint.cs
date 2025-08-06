using ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.Edit;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Knowledges.API.CharacterKnowledges.Edit;

public static class EditCharacterKnowledgeEndpoint
{
    public static async Task<Results<Ok, NotFound, ValidationProblem>> EditKnowledges(
        int characterId,
        int mappingId,
        [FromBody] EditCharacterKnowledgeRequest request,
        [FromServices] IUpdateKnowledgeForCharacterUseCase editKnowledgeUseCase
    )
    {
        var results = await editKnowledgeUseCase.ExecuteAsync(
            new UpdateKnowledgeForCharacterModel()
            {
                MappingId = mappingId,
                KnowledgeLevelId = request.KnowledgeLevelId,
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
