using ExpressedRealms.Events.API.UseCases.EventCheckin.CheckinUserInfo;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.EventCheckin.GetInitialCheckinUserInfo;

public static class GetGoCheckinInfoEndpoint
{
    public static async Task<Results<Ok<GetInitialCheckinUserInfoResponse>, ValidationProblem, NotFound>> 
        ExecuteAsync(
            string lookupId,
            [FromServices] IGetInitialCheckinUserInfoUseCase useCase)
    {
        var results = await useCase.ExecuteAsync(new GetInitialCheckinUserInfoModel()
        {
            LookupId = lookupId
        });

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(new GetInitialCheckinUserInfoResponse()
            {
                UserName = results.Value.Username,
                IsFirstTimeUser = results.Value.IsFirstTimeUser
            }
        );
    }
}
