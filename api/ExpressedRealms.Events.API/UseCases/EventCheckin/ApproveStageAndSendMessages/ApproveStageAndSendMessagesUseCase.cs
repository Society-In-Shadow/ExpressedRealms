using ExpressedRealms.DB.Models.Checkins.CheckinStageMappingSetup;
using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.ApproveStageAndSendMessages;

internal sealed class ApproveStageAndSendMessageUseCase(
    IEventCheckinRepository checkinRepository,
    IUserContext userContext,
    TimeProvider timeProvider,
    ApproveStageAndSendMessageModelValidator validator,
    CancellationToken cancellationToken
) : IApproveStageAndSendMessageUseCase
{
    public async Task<Result> ExecuteAsync(ApproveStageAndSendMessageModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);
        
        var eventId = await checkinRepository.GetActiveEventId();
        if (eventId is null)
            return Result.Fail("There are no active events to assign xp to");
        
        var playerId = await checkinRepository.GetPlayerId(model.LookupId);
        var checkin = await checkinRepository.GetCheckinAsync(eventId!.Value, playerId);
        
        if(checkin is null)
            return Result.Fail("Player has not checked in yet");
        
        var approvedStages = await checkinRepository.GetApprovedStages(checkin.Id);

        if (approvedStages.Any(x => x.CheckinStageId == model.StageId))
        {
            return Result.Fail("Stage has already been approved");
        }

        var nextStage = approvedStages.Count > 0 ? approvedStages.Max(x => x.Id) : 0;
        if (nextStage != model.StageId - 1)
            return Result.Fail("Stage is not next in line");

        await checkinRepository.CompleteStage(new CheckinStageMapping()
        {
            CreatedAt = timeProvider.GetUtcNow(),
            ApproverUserId = userContext.CurrentUserId(),
            CheckinStageId = model.StageId,
            CheckinId = checkin.Id
        });
        
        return Result.Ok();
    }
}
