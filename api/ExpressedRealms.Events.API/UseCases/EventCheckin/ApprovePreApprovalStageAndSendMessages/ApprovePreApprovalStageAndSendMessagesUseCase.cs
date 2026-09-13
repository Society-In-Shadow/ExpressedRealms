using ExpressedRealms.DB.Models.Checkins.CheckinSetup;
using ExpressedRealms.DB.Models.Checkins.CheckinStageMappingSetup;
using ExpressedRealms.DB.Models.Checkins.CheckinStageSetup;
using ExpressedRealms.Events.API.Discord;
using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.ApprovePreApprovalStageAndSendMessages;

internal sealed class ApprovePreApprovalStageAndSendMessageUseCase(
    IEventCheckinRepository checkinRepository,
    IUserContext userContext,
    TimeProvider timeProvider,
    IDiscordService discordService,
    ApprovePreApprovalStageAndSendMessageModelValidator validator,
    CancellationToken cancellationToken
) : IApprovePreApprovalStageAndSendMessageUseCase
{
    public async Task<Result> ExecuteAsync(ApprovePreApprovalStageAndSendMessageModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var eventId = await checkinRepository.GetExclusivePreCheckinEventId();
        if (eventId is null)
            return Result.Fail("There are no active events to approve");

        var playerId = await checkinRepository.GetPlayerIdFromCharacter(model.CharacterId);
        var checkin = await checkinRepository.GetCheckinAsync(eventId.Value, playerId);

        if (checkin is null)
            return Result.Fail("Player has not finalized their character");

        var stageRuleValidation = await StageRuleValidation(model, checkin);
        if (stageRuleValidation.IsFailed)
            return Result.Fail(stageRuleValidation.Errors);

        await checkinRepository.CompleteStage(
            new CheckinStageMapping()
            {
                CreatedAt = timeProvider.GetUtcNow(),
                ApproverUserId = userContext.CurrentUserId(),
                CheckinStageId = model.StageId,
                CheckinId = checkin.Id,
            }
        );

        if (model.StageId == CheckinStageEnum.GoApproval.Value)
        {
            // Create an archived copy of the primary character
            // This allows us to do diffs later
            await checkinRepository.CreatePrimaryCharacterArchiveAsync(playerId);

            // Once GO Approves, it immediately goes into CRB Creation
            await checkinRepository.CompleteStage(
                new CheckinStageMapping()
                {
                    CreatedAt = timeProvider.GetUtcNow(),
                    ApproverUserId = userContext.CurrentUserId(),
                    CheckinStageId = CheckinStageEnum.CrbCreation.Value,
                    CheckinId = checkin.Id,
                }
            );
            
            var seekingCrbMessage = $"A GO has Approved a Character for Print Out";
            await discordService.SendMessageToChannelAsync(
                DiscordChannel.PreCheckinLoadingBay,
                seekingCrbMessage
            );

        }

        return Result.Ok();
    }

    public static List<CheckinStageEnum> PreCheckinSequence =
    [
        CheckinStageEnum.PlayerEarlyCheckin,
        CheckinStageEnum.GoApproval,
        CheckinStageEnum.CrbCreation,
        CheckinStageEnum.PrintedCrb,
        CheckinStageEnum.CrbReadForPickup
    ];
    
    private async Task<Result> StageRuleValidation(
        ApprovePreApprovalStageAndSendMessageModel model,
        Checkin checkin
    )
    {
        var approvedStages = await checkinRepository.GetActiveApprovedStages(checkin.Id);
        var activeList = approvedStages.Select(x => CheckinStageEnum.FromValue(x.CheckinStageId)).ToList();

        if (activeList.Any(x => x == model.StageId))
        {
            return Result.Fail("Stage has already been approved");
        }

        var stage = CheckinStageEnum.FromValue(model.StageId);
        

        var currentStageIndex = PreCheckinSequence.FindLastIndex(activeList.Contains);

        var requestedStageIndex = PreCheckinSequence.IndexOf(stage);

        if (requestedStageIndex < 0)
            return Result.Fail("Stage is not relevant to this workflow");
        
        if (requestedStageIndex != currentStageIndex + 1)
        {
            return Result.Fail("Stage is not next in sequence.");
        }

        return Result.Ok();
    }
    
}
