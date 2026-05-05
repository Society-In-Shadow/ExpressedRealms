using ExpressedRealms.Powers.Repository.Powers;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Powers.API.PowerEndpoints.DeletePower;

public static class DeletePowerEndpoint
{
    public static async Task<Results<NotFound, NoContent, StatusCodeHttpResult>> Execute(
        int id,
        IPowerRepository repository
    )
    {
        var status = await repository.DeletePowerAsync(id);

        if (status.HasNotFound(out var notFound))
            return notFound;

        if (status.HasBeenDeletedAlready(out var deletedAlready))
            return deletedAlready;

        status.ThrowIfErrorNotHandled();

        return TypedResults.NoContent();
    }
}
