using ExpressedRealms.Events.API.UseCases.EventCheckin.UpdateAgeInformation;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.EventCheckin.UpdateAgeInfo;

public static class UpdateAgeInfoEndpoint
{
    public static async Task<Results<Ok, ValidationProblem, NotFound>> ExecuteAsync(
        string lookupId,
        [FromServices] IUpdateAgeInformationUseCase useCase,
        [FromBody] UpdateAgeInfoRequest model
    )
    {
        var results = await useCase.ExecuteAsync(
            new UpdateAgeInformationModel()
            {
                LookupId = Uri.UnescapeDataString(lookupId),
                AgeGroupId = model.AgeGroupId,
                HasSignedConsentForm = model.HasSignedConsentForm,
            }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok();
    }
}
