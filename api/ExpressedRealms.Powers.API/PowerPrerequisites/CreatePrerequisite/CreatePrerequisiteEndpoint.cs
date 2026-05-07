using ExpressedRealms.Powers.Repository.PowerPrerequisites.CreatePrerequisiteUseCase;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Powers.API.PowerPrerequisites.CreatePrerequisite;

public static class CreatePrerequisiteEndpoint
{
    public static async Task<Results<ValidationProblem, Ok>> Execute(
        int powerId,
        CreatePrerequisiteRequest request,
        [FromServices] ICreatePrerequisiteUseCase createCase
    )
    {
        var results = await createCase.ExecuteAsync(
            new CreatePrerequisiteModel()
            {
                PowerId = powerId,
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
