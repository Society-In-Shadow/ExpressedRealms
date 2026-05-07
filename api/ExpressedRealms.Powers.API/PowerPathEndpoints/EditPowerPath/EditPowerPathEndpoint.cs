using ExpressedRealms.Powers.Repository.PowerPaths;
using ExpressedRealms.Powers.Repository.PowerPaths.DTOs.PowerPathEdit;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Powers.API.PowerPathEndpoints.EditPowerPath;

public static class EditPowerPathEndpoint
{
    public static async Task<Results<ValidationProblem, NotFound, NoContent>> Execute(
        int id,
        EditPowerPathRequest request,
        IPowerPathRepository repository
    )
    {
        var results = await repository.EditPowerPathAsync(
            new EditPowerPathModel()
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
            }
        );

        if (results.HasNotFound(out var notFound))
        {
            return notFound;
        }

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;

        results.ThrowIfErrorNotHandled();

        return TypedResults.NoContent();
    }
}
