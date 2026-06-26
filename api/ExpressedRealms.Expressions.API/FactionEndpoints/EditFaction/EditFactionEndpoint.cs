using ExpressedRealms.Expressions.UseCases.FactionUseCases.EditFaction;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Expressions.API.FactionEndpoints.EditFaction;

public static class EditFactionEndpoint
{
    public static async Task<Results<Ok, NotFound, ValidationProblem>> ExecuteAsync(
        int id,
        [FromBody] EditFactionRequest request,
        [FromServices] IEditFactionUseCase editFactionUseCase
    )
    {
        var results = await editFactionUseCase.ExecuteAsync(
            new EditFactionModel()
            {
                Id = id,
                Name = request.Name,
                Background = request.Background,
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
