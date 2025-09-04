using ExpressedRealms.Blessings.UseCases.Blessings.DeleteBlessing;
using ExpressedRealms.Knowledges.UseCases.Knowledges.DeleteKnowledge;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Blessings.API.Blessings.DeleteBlessing;

public static class DeleteBlessingEndpoint
{
    public static async Task<Results<Ok, NotFound, ValidationProblem>> Execute(
        int id,
        [FromServices] IDeleteBlessingUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(new DeleteBlessingModel() { Id = id });

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok();
    }
}
