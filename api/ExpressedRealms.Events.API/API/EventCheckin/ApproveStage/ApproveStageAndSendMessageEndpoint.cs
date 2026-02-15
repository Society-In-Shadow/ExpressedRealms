using ExpressedRealms.Events.API.UseCases.EventCheckin.ApproveStageAndSendMessages;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.EventCheckin.ApproveStage;

public static class ApproveStageAndSendMessageEndpoint
{
    public static async Task<Results<Ok, ValidationProblem, NotFound>> ExecuteAsync(
        string lookupId,
        [FromBody] ApproveStageAndSendMessageRequest request,
        [FromServices] IApproveStageAndSendMessageUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new ApproveStageAndSendMessageModel()
            {
                LookupId = lookupId,
                StageId = request.StageId
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
