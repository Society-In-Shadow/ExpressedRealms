using ExpressedRealms.Blessings.UseCases.Blessings.EditBlessings;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Blessings.API.Blessings.EditBlessing;

public static class EditBlessingEndpoint
{
    public static async Task<Results<Ok, NotFound, ValidationProblem>> Execute(
        int id,
        [FromBody] EditBlessingRequest request,
        [FromServices] IEditBlessingUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new EditBlessingModel()
            {
                Name = request.Name,
                Description = request.Description,
                SubCategory = request.Category,
                Type = request.Type,
                Id = id,
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
