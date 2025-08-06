using ExpressedRealms.Knowledges.API.CharacterKnowledges.Edit;
using ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.Edit;
using ExpressedRealms.Knowledges.UseCases.KnowledgeSpecializations.EditSpecialization;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Knowledges.API.KnowledgeSpecializations.Edit;

public static class EditCharacterSpecializationEndpoint
{
    public static async Task<Results<Ok, NotFound, ValidationProblem>> Edit(
        int characterId,
        int mappingId,
        int specializationId,
        [FromBody] EditCharacterSpecializationRequest request,
        [FromServices] IEditSpecializationUseCase editKnowledgeUseCase
    )
    {
        var results = await editKnowledgeUseCase.ExecuteAsync(
            new EditSpecializationModel()
            {
                Id = specializationId,
                Description = request.Description,
                Name = request.Name,
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
