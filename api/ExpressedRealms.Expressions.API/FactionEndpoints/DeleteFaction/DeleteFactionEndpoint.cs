using ExpressedRealms.Expressions.UseCases.FactionUseCases.DeleteFaction;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Expressions.API.FactionEndpoints.DeleteFaction;

public static class DeleteFactionEndpoint
{
    public static async Task<Results<Ok, NotFound, ValidationProblem>> ExecuteAsync(
        int id,
        [FromServices] IDeleteFactionUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(new DeleteFactionModel() { Id = id });

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok();
    }
}
