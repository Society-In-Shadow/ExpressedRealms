using ExpressedRealms.Blessings.UseCases.Blessings.CreateBlessings;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Blessings.API.Blessings.CreateBlessing;

public static class CreateBlessingEndpoint
{
    public static async Task<Results<Ok<int>, NotFound, ValidationProblem>> Execute(
        [FromBody] CreateBlessingRequest request,
        [FromServices] ICreateBlessingUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new CreateBlessingModel()
            {
                Name = request.Name,
                Description = request.Description,
                SubCategory = request.Category,
                Type = request.Type,
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
