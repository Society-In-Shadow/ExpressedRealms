using ExpressedRealms.Powers.Repository.PowerPaths;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Powers.API.PowerPathEndpoints.DeletePowerPath;

public static class DeletePowerPathEndpoint
{
    public static async Task<Results<NotFound, NoContent, StatusCodeHttpResult>> Execute(
        int id,
        IPowerPathRepository repository
    )
    {
        var status = await repository.DeletePowerPathAsync(id);

        if (status.HasNotFound(out var notFound))
            return notFound;

        if (status.HasBeenDeletedAlready(out var deletedAlready))
            return deletedAlready;

        status.ThrowIfErrorNotHandled();

        return TypedResults.NoContent();
    }
}
