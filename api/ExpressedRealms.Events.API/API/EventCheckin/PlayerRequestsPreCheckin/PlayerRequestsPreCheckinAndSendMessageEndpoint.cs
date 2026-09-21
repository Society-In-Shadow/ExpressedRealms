using ExpressedRealms.Events.API.UseCases.EventCheckin.PlayerRequestsPreCheckin;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.EventCheckin.PlayerRequestsPreCheckin;

public static class PlayerRequestedPreCheckinEndpoint
{
    public static async Task<
        Results<Ok, ValidationProblem, NotFound, ForbidHttpResult>
    > ExecuteAsync([FromServices] IPlayerRequestsPreCheckinUseCase useCase)
    {
        var results = await useCase.ExecuteAsync();

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok();
    }
}
