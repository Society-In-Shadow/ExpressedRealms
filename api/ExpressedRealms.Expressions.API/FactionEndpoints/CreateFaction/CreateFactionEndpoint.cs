using ExpressedRealms.Expressions.UseCases.FactionUseCases.CreateFaction;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Expressions.API.FactionEndpoints.CreateFaction;

public static class CreateFactionEndpoint
{
    public static async Task<Results<Ok<int>, NotFound, ValidationProblem>> ExecuteAsync(
        [FromBody] CreateFactionRequest request,
        [FromServices] ICreateFactionUseCase createFactionUseCase
    )
    {
        var results = await createFactionUseCase.ExecuteAsync(
            new CreateFactionModel()
            {
                Name = request.Name,
                Background = request.Background,
                ExpressionId = request.ExpressionId,
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
