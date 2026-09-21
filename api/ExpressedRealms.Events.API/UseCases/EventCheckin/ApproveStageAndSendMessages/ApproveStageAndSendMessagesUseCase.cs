using ExpressedRealms.DB.Models.Checkins.CheckinStageMappingSetup;
using ExpressedRealms.DB.Models.Checkins.CheckinStageSetup;
using ExpressedRealms.Email.EmailClientAdapter;
using ExpressedRealms.Events.API.Discord;
using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.ApproveStageAndSendMessages;

internal sealed class ApproveStageAndSendMessageUseCase(
    IEventCheckinRepository checkinRepository,
    IUserContext userContext,
    TimeProvider timeProvider,
    IDiscordService discordService,
    IEmailClientAdapter emailSender,
    ApproveStageAndSendMessageModelValidator validator,
    CancellationToken cancellationToken
) : IApproveStageAndSendMessageUseCase
{
    public static readonly List<CheckinStageEnum> PreCheckinSequence =
    [
        CheckinStageEnum.PlayerEarlyCheckin,
        CheckinStageEnum.GoApproval,
        CheckinStageEnum.CrbPrinted,
        CheckinStageEnum.CrbAssembled,
        CheckinStageEnum.CrbPickedUp
    ];

    public static readonly List<CheckinStageEnum> InitialCheckinSequence =
    [
        CheckinStageEnum.AgeCheckApproval,
        CheckinStageEnum.EventQuestionsCheck,
        CheckinStageEnum.CharacterStorageQuestion,
        CheckinStageEnum.AssignedXpCheck,
        CheckinStageEnum.GoApproval, 
        CheckinStageEnum.CrbPrinted,
        CheckinStageEnum.CrbAssembled,
        CheckinStageEnum.CrbPickedUp,
        CheckinStageEnum.Day2Checkin,
        CheckinStageEnum.Day3Checkin,
        CheckinStageEnum.FinalStage
    ];
    
    public async Task<Result> ExecuteAsync(ApproveStageAndSendMessageModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        // TODO: Update This
        var eventId = await checkinRepository.GetInclusivePreCheckinEventId();
        if (eventId is null)
            return Result.Fail("There are no active events to checkin into");

        Guid playerId;
        if (model.LookupId is not null)
        {
            var retrievedPlayerId = await checkinRepository.GetPlayerIdOrDefault(model.LookupId);
            if (retrievedPlayerId is null)
                return ValidationHelper.AddSingleValidationFailure(nameof(model.LookupId), "Lookup Id does not exist");

            playerId = retrievedPlayerId.Value;
        }
        else
        {
            var retrievedPlayerId = await checkinRepository.GetPlayerIdFromCharacter(model.CharacterId!.Value);
            if (retrievedPlayerId is null)
                return ValidationHelper.AddSingleValidationFailure(nameof(model.CharacterId),
                    "Character Id does not exist");
            playerId = retrievedPlayerId.Value;
        }

        var checkin = await checkinRepository.GetCheckinAsync(eventId.Value, playerId);

        if (checkin is null)
            return Result.Fail("Player has not checked in yet");

        var requestedStage = CheckinStageEnum.FromValue(model.StageId);
        var sequenceData = await GetSequenceData(checkin.Id, requestedStage);

        var hasBeenCompleted = sequenceData.CompletedStages.Contains(requestedStage);
        if (hasBeenCompleted)
            return Result.Fail("Stage has been completed already");

        var inPreCheckinPeriod = await checkinRepository.GetExclusivePreCheckinEventId();
        if (inPreCheckinPeriod.HasValue && !PreCheckinSequence.Contains(requestedStage))
        {
            return Result.Fail("Precheckin Period doesn't allow this type of stage to be completed.");
        }
        
        if (requestedStage == CheckinStageEnum.PlayerEarlyCheckin)
        {
            // Check to make sure they have early checkin priveleges
            var hasEarlyPermission = false;
            if (hasEarlyPermission)
            {
                await CompleteStage(CheckinStageEnum.PlayerEarlyCheckin, checkin.Id);
                // Short Circuit, this is all that needs to happen with this stage
                return Result.Ok();                
            }
            
            return Result.Fail("The user does not have Early Checkin Privileges");

        }
        
        var earlyCheckinBypass = sequenceData.CompletedStages.Contains(CheckinStageEnum.PlayerEarlyCheckin);
        if (requestedStage == CheckinStageEnum.GoApproval && (sequenceData.PreviousStageComplete || earlyCheckinBypass))
        {
            // Create an archived copy of the primary character
            // This allows us to do diffs later
            await checkinRepository.CreatePrimaryCharacterArchiveAsync(playerId);

            // Once GO Approves, it immediately goes into CRB Creation
            await CompleteStage(requestedStage, checkin.Id);

            if (earlyCheckinBypass)
            {
                var seekingCrbMessage = $"A GO has Approved a Character for Print Out";
                await discordService.SendMessageToChannelAsync(
                    DiscordChannel.PreCheckinLoadingBay,
                    seekingCrbMessage
                );
            }
            else
            {
                var seekingCrbMessage = $"A CRB was approved and put into the print queue";
                await discordService.SendMessageToChannelAsync(
                    DiscordChannel.PlayersSeekingCrbs,
                    seekingCrbMessage
                );                
            }

            return Result.Ok();
        }

        if (requestedStage == CheckinStageEnum.AgeCheckApproval)
        {
            // First stage always gets automatically approved
            await CompleteStage(model.StageId, checkin.Id);
            return Result.Ok();
        }
        
        // Default rule, previous Stage needs to have existed before approving this one
        if (!sequenceData.PreviousStageComplete)
            return Result.Fail("Previous stage has not been completed");        

        // Automatically approve the stage, as the only rules going forward add missing steps after this one
        await CompleteStage(model.StageId, checkin.Id);
        
        if (model.StageId == CheckinStageEnum.CrbAssembled.Value)
        {
            await SendPickupCrbEmailIfNeeded(playerId);
        }

        var currentDay = await checkinRepository.GetCurrentEventDay();
        if (model.StageId == CheckinStageEnum.CrbPickedUp.Value && currentDay >= 2)
        {
            // Automatically go to day 2
            await CompleteStage(requestedStage, checkin.Id);
            await CompleteStage(CheckinStageEnum.Day2Checkin, checkin.Id);
        }

        if (model.StageId == CheckinStageEnum.CrbPickedUp.Value && currentDay >= 3)
        {
            await CompleteStage(requestedStage, checkin.Id);
            await CompleteStage(CheckinStageEnum.Day2Checkin, checkin.Id);
            await CompleteStage(CheckinStageEnum.Day3Checkin, checkin.Id);
        }

        return Result.Ok();
    }

    private async Task CompleteStage(int stageId, int checkinId)
    {
        await checkinRepository.CompleteStage(
            new CheckinStageMapping()
            {
                CreatedAt = timeProvider.GetUtcNow(),
                ApproverUserId = userContext.CurrentUserId(),
                CheckinStageId = stageId,
                CheckinId = checkinId,
            }
        );            
    }

    private record SequenceData(
        List<CheckinStageEnum> CompletedStages,
        bool PreviousStageComplete);
    
    private async Task<SequenceData> GetSequenceData(
        int checkinId,
        CheckinStageEnum currentTargetStage
    )
    {
        // Filters stages, makes sure that stages between initial checkin and anything before reapproval gets removed
        var activeList = await checkinRepository.GetActiveApprovedStages(checkinId);

        var completedStages = activeList.Select(x => CheckinStageEnum.FromValue(x.CheckinStageId)).ToList();

        if (completedStages.Count == 0)
            return new SequenceData(completedStages, false);

        var requestedStageIndex = InitialCheckinSequence.IndexOf(currentTargetStage);
        
        var previousStage = InitialCheckinSequence
            .Where((x, y) => y == requestedStageIndex - 1)
            .FirstOrDefault();

        var previousStepCompleted = previousStage is not null && completedStages.Contains(previousStage);
        return new SequenceData(completedStages, previousStepCompleted);
    }


    private async Task SendPickupCrbEmailIfNeeded(Guid playerId)
    {
        var emailPreferenceInfo =
            await checkinRepository.GetPlayerCrbEmailPreferenceWithPlayerNumber(playerId);
        if (emailPreferenceInfo.SendPickupCrbEmail)
        {
            await emailSender.SendEmailAsync(
                new EmailData(
                    emailPreferenceInfo.UserEmailAddress,
                    "CRB is Ready for Pickup!",
                    @"Hello!

Your CRB is ready for pickup!  Feel free to stop by SHQ once you are ready to pick it up.

Thanks,
Order of Archivists
Society in Shadows
",
                    $"""
                     <p>Hello!</p>

                     <p>Your CRB is ready for pickup!  Feel free to stop by SHQ once you are ready to pick it up.</p>

                     <p>Thanks,</p>
                     <p>Order of Archivists</p>
                     <p>Society in Shadows</p>
                     """
                )
            );
        }
    }
}
