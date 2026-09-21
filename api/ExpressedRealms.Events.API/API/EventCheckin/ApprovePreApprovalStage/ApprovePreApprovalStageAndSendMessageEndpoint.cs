using System.Security.Claims;
using ExpressedRealms.Authentication.PermissionCollection;
using ExpressedRealms.DB.Models.Checkins.CheckinStageSetup;
using ExpressedRealms.Events.API.UseCases.EventCheckin.ApproveStageAndSendMessages;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.EventCheckin.ApprovePreApprovalStage;

public static class ApprovePreApprovalStageEndpoint
{
    public static async Task<
        Results<Ok, ValidationProblem, NotFound, ForbidHttpResult>
    > ExecuteAsync(
        int characterId,
        ClaimsPrincipal user,
        [FromBody] ApprovePreApprovalStageRequest request,
        [FromServices] IApproveStageAndSendMessageUseCase useCase
    )
    {
        List<int> crbCreationStages =
        [
            CheckinStageEnum.CrbPrinted.Value,
            CheckinStageEnum.CrbPickedUp.Value,
            CheckinStageEnum.CrbReadForPickup.Value,
        ];

        if (
            crbCreationStages.Contains(request.StageId)
            && !user.HasClaim("custom_permission", Permissions.Event.CrbHandling.Key)
        )
        {
            return TypedResults.Forbid();
        }

        if (
            request.StageId == CheckinStageEnum.GoApproval.Value
            && !user.HasClaim("custom_permission", Permissions.Event.GoApproval.Key)
        )
        {
            return TypedResults.Forbid();
        }

        var results = await useCase.ExecuteAsync(
            new() { StageId = request.StageId, CharacterId = characterId }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok();
    }
}
