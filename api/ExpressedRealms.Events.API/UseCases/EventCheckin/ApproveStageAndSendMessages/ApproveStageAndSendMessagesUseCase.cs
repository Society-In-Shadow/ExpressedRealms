using ExpressedRealms.DB.Models.Checkins.CheckinSetup;
using ExpressedRealms.DB.Models.Checkins.CheckinStageMappingSetup;
using ExpressedRealms.DB.Models.Checkins.CheckinStageSetup;
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

        if (checkin is null)
            return Result.Fail("Player has not checked in yet");

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

        // This should be a separate Use Case, though it would be doing nothing
        // Other then this for now.
        // Later down the road, it would prevent modification of the character
        if (model.StageId == CheckinStageEnum.GoApproval.Value)
        {
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
        }

        if (model.StageId == CheckinStageEnum.AssignedXpCheck.Value)
        {
            // Once applied, it goes into GO Check stage
            await checkinRepository.CompleteStage(
                new CheckinStageMapping()
                {
                    CreatedAt = timeProvider.GetUtcNow(),
                    ApproverUserId = userContext.CurrentUserId(),
                    CheckinStageId = CheckinStageEnum.ShqApproval.Value,
                    CheckinId = checkin.Id,
                }
            );
        }
        
        var currentDay = await checkinRepository.GetCurrentEventDay();
        if(model.StageId == CheckinStageEnum.CrbPickedUp.Value && currentDay >= 2)
        {
            // Automatically bypass day 2
            await checkinRepository.CompleteStage(
                new CheckinStageMapping()
                {
                    CreatedAt = timeProvider.GetUtcNow(),
                    ApproverUserId = userContext.CurrentUserId(),
                    CheckinStageId = CheckinStageEnum.Day2Checkin.Value,
                    CheckinId = checkin.Id,
                }
            );
        }
        
        if(model.StageId == CheckinStageEnum.CrbPickedUp.Value && currentDay >= 3)
        {
            // Automatically bypass day 3
            await checkinRepository.CompleteStage(
                new CheckinStageMapping()
                {
                    CreatedAt = timeProvider.GetUtcNow(),
                    ApproverUserId = userContext.CurrentUserId(),
                    CheckinStageId = CheckinStageEnum.Day3Checkin.Value,
                    CheckinId = checkin.Id,
                }
            );
        }

        await SendMessages(model, playerId);

        return Result.Ok();
    }

    private async Task<Result<bool>> StageRuleValidation(
        ApproveStageAndSendMessageModel model,
        Checkin checkin
    )
    {
        List<CheckinStageMapping> activeList = [];

        await GetActiveApprovedStages(checkin, activeList);

        var hasBeenPickedUp = activeList.Any(x =>
            x.CheckinStageId == CheckinStageEnum.CrbPickedUp.Value
        );
        if (model.StageId == CheckinStageEnum.PlayerNeedsReapproval.Value && !hasBeenPickedUp)
        {
            return Result.Fail("Player needs to pick up their CRB before they can re-approve");
        }

        activeList = activeList
            .Where(x => x.CheckinStageId != CheckinStageEnum.PlayerNeedsReapproval.Value)
            .ToList();

        if (activeList.Any(x => x.CheckinStageId == model.StageId))
        {
            return Result.Fail("Stage has already been approved");
        }

        var dayCheckins = new[] { 6, 7 };

        var currentStage =
            activeList.Count > 0 ? activeList.MaxBy(x => x.CreatedAt)!.CheckinStageId : 0;
        var stage = CheckinStageEnum.FromValue(model.StageId);

        // ---- Rule 0: If first stage, skip the rest of the rules
        if (stage != CheckinStageEnum.AgeCheckApproval)
        {
            var currentStageEnum = CheckinStageEnum.FromValue(currentStage);
            // ---- Rule 1: Sequential for stages 1–9 ----
            if (stage.SortOrder <= 9 && stage.SortOrder != currentStageEnum.SortOrder + 1)
            {
                return Result.Fail("Stage is not next in sequence.");
            }

            // ---- Rule 2: Stage 10 & 11 locked until 1–5 complete ----
            if (dayCheckins.Contains(model.StageId))
            {
                var completedStageIds = activeList.Select(x => CheckinStageEnum.FromValue(x.CheckinStageId).SortOrder).ToList();
                bool firstFiveComplete = Enumerable
                    .Range(1, 9)
                    .All(stageId => completedStageIds.Contains(stageId));

                if (!firstFiveComplete)
                {
                    return Result.Fail(
                        "Stages 1 through 9 must be completed before day check-ins."
                    );
                }
            }
        }

        return false;
    }

    private async Task GetActiveApprovedStages(
        Checkin checkin,
        List<CheckinStageMapping> activeList
    )
    {
        var approvedStages = await checkinRepository.GetApprovedStages(checkin.Id);

        var latestReapprovedStage = approvedStages
            .Where(x => x.CheckinStageId == CheckinStageEnum.PlayerNeedsReapproval.Value)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefault();

        if (latestReapprovedStage is not null)
        {
            var stagesThatCannotBeReapproved = new[]
            {
                CheckinStageEnum.AgeCheckApproval.Value,
                CheckinStageEnum.EventQuestionsCheck.Value,
                CheckinStageEnum.EventQuestionsCheck.Value,
                CheckinStageEnum.AssignedXpCheck.Value,
                CheckinStageEnum.ShqApproval.Value,
            };

            activeList.AddRange(
                approvedStages.Where(x => stagesThatCannotBeReapproved.Contains(x.CheckinStageId))
            );
            activeList.AddRange(
                approvedStages
                    .Where(x => x.CreatedAt >= latestReapprovedStage.CreatedAt)
                    .OrderByDescending(x => x.CreatedAt)
                    .ToList()
            );
        }
        else
        {
            activeList.AddRange(approvedStages);
        }
    }

    private async Task SendMessages(ApproveStageAndSendMessageModel model, Guid playerId)
    {
        var playerName = await checkinRepository.GetPlayerNameWithPlayerNumber(model.LookupId);
        var primaryCharacterInfo = await checkinRepository.GetPrimaryCharacterInformation(playerId);
        var approverName = await checkinRepository.GetCurrentPlayerName();

        switch (model.StageId)
        {
            case 1: // SHQ Approval
            case 12: // Reapprove A Character
            {
                var seekingGoMessage = "";

                if (primaryCharacterInfo is not null)
                    seekingGoMessage =
                        $"\"{playerName}\" with character \"{primaryCharacterInfo.CharacterName}\" needs a GO ~{approverName}";
                else
                    seekingGoMessage =
                        $"\"{playerName}\" with no primary character needs a GO ~{approverName}";

                await discordService.SendMessageToChannelAsync(
                    DiscordChannel.PlayersSeekingGos,
                    seekingGoMessage
                );
                break;
            }
            case 2: // GO Approval
            {
                var seekingGoMessage =
                    $"\"{playerName}\" with character \"{primaryCharacterInfo!.CharacterName}\" was approved by GO ~{approverName}";
                await discordService.SendMessageToChannelAsync(
                    DiscordChannel.PlayersSeekingGos,
                    seekingGoMessage
                );

                var seekingCrbMessage =
                    $"\"{playerName}\" with character \"{primaryCharacterInfo.CharacterName}\" needs a CRB Printed ~{approverName}";
                await discordService.SendMessageToChannelAsync(
                    DiscordChannel.PlayersSeekingCrbs,
                    seekingCrbMessage
                );
                break;
            }
            case 4: // Crb Approval
            {
                var seekingCrbMessage =
                    $"\"{playerName}\" with character \"{primaryCharacterInfo!.CharacterName}\" has a CRB ready for pickup ~{approverName}";
                await discordService.SendMessageToChannelAsync(
                    DiscordChannel.PlayersSeekingCrbs,
                    seekingCrbMessage
                );
                break;
            }

            case 5: // Crb Finalized
            {
                var seekingCrbMessage =
                    $"\"{playerName}\" with character \"{primaryCharacterInfo!.CharacterName}\" picked up their CRB ~{approverName}";
                await discordService.SendMessageToChannelAsync(
                    DiscordChannel.PlayersSeekingCrbs,
                    seekingCrbMessage
                );
                break;
            }
        }
    }
}
