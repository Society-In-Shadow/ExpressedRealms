using ExpressedRealms.Powers.Repository.PowerPaths;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Powers.API.PowerPathEndpoints.GetPowerPath;

public static class GetPowerPathEndpoint
{
    public static async Task<Results<NotFound, Ok<EditPowerPathResponse>>> Execute(
        int id,
        IPowerPathRepository powerRepository
    )
    {
        var powers = await powerRepository.GetPowerPathAsync(id);

        if (powers.HasNotFound(out var notFound))
            return notFound;

        return TypedResults.Ok(
            new EditPowerPathResponse()
            {
                Id = powers.Value.Id,
                Name = powers.Value.Name,
                Description = powers.Value.Description,
            }
        );
    }
}
