using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Admin.API.ArchetypeEndpoints.DeleteArchetype;

public static class DeleteArchetypeEndpoint
{
    public static async Task<Results<Ok, ValidationProblem, NotFound>> ExecuteAsync(
        int id,
        ICharacterRepository repository
    )
    {
        var results = await repository.DeleteCharacterAsync(id);

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;
        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok();
    }
}
