using ExpressedRealms.Powers.Repository.PowerPaths;
using ExpressedRealms.Powers.Repository.PowerPaths.DTOs.PowerPathCreate;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Powers.API.PowerPathEndpoints.CreatePowerPath;

public static class CreatePowerPathEndpoint
{
    public static async Task<Results<ValidationProblem, NotFound, Created<int>>> Execute(
        CreatePowerPathRequest request,
        IPowerPathRepository repository
    )
    {
        var results = await repository.CreatePowerPathAsync(
            new CreatePowerPathModel()
            {
                Name = request.Name,
                Description = request.Description,
                ExpressionId = request.ExpressionId,
            }
        );

        if (results.HasNotFound(out var notFound))
        {
            return notFound;
        }

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Created("/powers", results.Value);
    }
}
