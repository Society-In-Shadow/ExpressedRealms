using ExpressedRealms.DB.Models.Checkins.CheckinStageMappingSetup;
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

        var approvedStages = await checkinRepository.GetApprovedStages(checkin.Id);

        if (approvedStages.Any(x => x.CheckinStageId == model.StageId))
        {
            return Result.Fail("Stage has already been approved");
        }

        var dayCheckins = new[] { 6, 7 };
        var nextStage = approvedStages.Count > 0 ? approvedStages.Max(x => x.CheckinStageId) : 0;
        var completedStageIds = approvedStages.Select(x => x.CheckinStageId).ToList();

        // ---- Rule 1: Sequential for stages 1–5 ----
        if (model.StageId <= 5 && model.StageId != nextStage + 1)
            return Result.Fail("Stage is not next in sequence.");

        // ---- Rule 2: Stage 6 & 7 locked until 1–5 complete ----
        if (dayCheckins.Contains(model.StageId))
        {
            bool firstFiveComplete = Enumerable
                .Range(1, 5)
                .All(stage => completedStageIds.Contains(stage));

            if (!firstFiveComplete)
                return Result.Fail("Stages 1 through 5 must be completed before day check-ins.");
        }

        await checkinRepository.CompleteStage(
            new CheckinStageMapping()
            {
                CreatedAt = timeProvider.GetUtcNow(),
                ApproverUserId = userContext.CurrentUserId(),
                CheckinStageId = model.StageId,
                CheckinId = checkin.Id,
            }
        );

        if (model.StageId == 2)
        {
            // Once GO Approves, it immediately goes into CRB Creation
            await checkinRepository.CompleteStage(
                new CheckinStageMapping()
                {
                    CreatedAt = timeProvider.GetUtcNow(),
                    ApproverUserId = userContext.CurrentUserId(),
                    CheckinStageId = model.StageId + 1,
                    CheckinId = checkin.Id,
                }
            );
        }

        await SendMessages(model, playerId);

        return Result.Ok();
    }

    private async Task SendMessages(ApproveStageAndSendMessageModel model, Guid playerId)
    {
        var playerName = await checkinRepository.GetPlayerNameWithPlayerNumber(model.LookupId);
        var primaryCharacterInfo = await checkinRepository.GetPrimaryCharacterInformation(playerId);
        var approverName = await checkinRepository.GetCurrentPlayerName();

        switch (model.StageId)
        {
            case 1: // SHQ Approval
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
