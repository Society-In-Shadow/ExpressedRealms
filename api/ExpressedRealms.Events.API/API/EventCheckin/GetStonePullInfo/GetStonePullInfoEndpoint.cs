using ExpressedRealms.Events.API.UseCases.EventCheckin.GetStonePullInfo;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.EventCheckin.GetStonePullInfo;

public static class GetStonePullInfoEndpoint
{
    public static async Task<
        Results<Ok<GetGoCheckinInfoResponse>, ValidationProblem, NotFound>
    > ExecuteAsync(string lookupId, [FromServices] IGetStonePullInfoUseCase useCase)
    {
        var results = await useCase.ExecuteAsync(new() { LookupId = lookupId });

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(
            new GetGoCheckinInfoResponse()
            {
                HasCompletedStep = results.Value.HasCompletedStep,
                IsFirstTimeUser = results.Value.IsFirstTimeUser,
                BroughtFriend = results.Value.BroughtFriend,
                AssignedXp = results.Value.AssignedXp is null
                    ? null
                    : new AssignedXpType()
                    {
                        Amount = results.Value.AssignedXp.Amount,
                        TypeId = results.Value.AssignedXp.TypeId,
                    },
            }
        );
    }
}
