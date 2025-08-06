using ExpressedRealms.Knowledges.UseCases.KnowledgeSpecializations.CreateSpecialization;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Knowledges.API.KnowledgeSpecializations.Create;

public static class CreateKnowledgeSpecializationEndpoint
{
    public static async Task<
        Results<Ok<int>, NotFound, ValidationProblem, BadRequest<string>>
    > Create(
        int characterId,
        int mappingId,
        [FromBody] CreateKnowledgeSpecializationRequest request,
        [FromServices] ICreateSpecializationUseCase createKnowledgeUseCase
    )
    {
        var results = await createKnowledgeUseCase.ExecuteAsync(
            new CreateSpecializationModel()
            {
                KnowledgeMappingId = mappingId,
                Description = request.Description,
                Name = request.Name,
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
