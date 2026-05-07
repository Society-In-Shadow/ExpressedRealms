using ExpressedRealms.Powers.Repository.PowerPrerequisites.DeletePrerequisiteUseCase;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Powers.API.PowerPrerequisites.DeletePrerequisite;

public static class DeletePrerequisiteEndpoint
{
    public static async Task<Results<ValidationProblem, Ok>> Execute(
        int powerId,
        int prerequisiteId,
        [FromServices] IDeletePrerequisiteUseCase deleteCase
    )
    {
        var results = await deleteCase.ExecuteAsync(
            new DeletePrerequisiteModel() { Id = prerequisiteId }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok();
    }
}
