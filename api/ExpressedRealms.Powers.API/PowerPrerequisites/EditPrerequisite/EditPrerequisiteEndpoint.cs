using ExpressedRealms.Powers.Repository.PowerPrerequisites.EditPrerequisiteUseCase;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Powers.API.PowerPrerequisites.EditPrerequisite;

public static class EditPrerequisiteEndpoint
{
    public static async Task<Results<ValidationProblem, Ok>> Execute(
        int powerId,
        int prerequisiteId,
        EditPrerequisiteRequest request,
        [FromServices] IEditPrerequisiteUseCase editCase
    )
    {
        var results = await editCase.ExecuteAsync(
            new EditPrerequisiteModel()
            {
                Id = prerequisiteId,
                RequiredAmount = request.RequiredAmount,
                PrerequisitePowerIds = request.PowerIds,
            }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok();
    }
}
