using ExpressedRealms.Powers.Repository.PowerPrerequisites.GetPrerequisiteUseCase;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Powers.API.PowerPrerequisites.GetPrerequisite;

public static class GetPrerequisiteEndpoint
{
    public static async Task<Results<ValidationProblem, Ok<GetPrerequisiteResponse?>>> Execute(
        int powerId,
        [FromServices] IGetPrerequisiteUseCase getCase
    )
    {
        var results = await getCase.ExecuteAsync(
            new GetPrerequisiteModel() { PowerId = powerId }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;

        results.ThrowIfErrorNotHandled();

        if (results.Value is null)
        {
            return TypedResults.Ok<GetPrerequisiteResponse?>(null);
        }

        return TypedResults.Ok<GetPrerequisiteResponse?>(
            new GetPrerequisiteResponse()
            {
                Id = results.Value.Id,
                RequiredAmount = results.Value.RequiredAmount,
                PowerIds = results.Value.PowerIds,
            }
        );
    }
}
