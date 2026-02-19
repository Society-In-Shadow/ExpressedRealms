using System.Security.Claims;
using ExpressedRealms.Authentication.PermissionCollection;
using ExpressedRealms.DB.Models.Checkins.CheckinStageSetup;
using ExpressedRealms.Events.API.UseCases.EventCheckin.ApproveStageAndSendMessages;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.EventCheckin.ApproveStage;

public static class ApproveStageAndSendMessageEndpoint
{
    public static async Task<Results<Ok, ValidationProblem, NotFound, ForbidHttpResult>> ExecuteAsync(
        string lookupId,
        ClaimsPrincipal user,
        [FromBody] ApproveStageAndSendMessageRequest request,
        [FromServices] IApproveStageAndSendMessageUseCase useCase
    )
    {
        List<int> crbCreationStages =
        [
            CheckinStageEnum.CrbCreation.Value, CheckinStageEnum.CrbPickedUp.Value,
            CheckinStageEnum.CrbReadForPickup.Value
        ];
        
        if (crbCreationStages.Contains(request.StageId) && 
            !user.HasClaim("custom_permission", Permissions.Event.CrbHandling.Key))
        {
            return TypedResults.Forbid();
        }
        
        List<int> day23CheckinStages =
        [
            CheckinStageEnum.Day2Checkin.Value, CheckinStageEnum.Day3Checkin.Value
        ];
        
        if (day23CheckinStages.Contains(request.StageId) && 
            !user.HasClaim("custom_permission", Permissions.Event.Day23Checkin.Key))
        {
            return TypedResults.Forbid();
        }
        
        if (request.StageId == CheckinStageEnum.GoApproval.Value && 
            !user.HasClaim("custom_permission", Permissions.Event.GoApproval.Key))
        {
            return TypedResults.Forbid();
        }
        
        var results = await useCase.ExecuteAsync(
            new ApproveStageAndSendMessageModel() { LookupId = lookupId, StageId = request.StageId }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok();
    }
}
