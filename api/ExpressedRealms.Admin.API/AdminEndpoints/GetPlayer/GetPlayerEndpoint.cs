using ExpressedRealms.Admin.UseCases.Users.GetPlayer;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Admin.API.AdminEndpoints.GetPlayer;

public static class GetPlayerEndpoint
{
    public static async Task<
        Results<Ok<GetPlayerResponse>, ValidationProblem, NotFound>
    > ExecuteAsync(Guid userId, [FromServices] IGetPlayerUseCase useCase)
    {
        var results = await useCase.ExecuteAsync(new GetPlayerModel() { Id = userId });

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;
        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(
            new GetPlayerResponse() { PlayerNumber = results.Value.PlayerNumber }
        );
    }
}
