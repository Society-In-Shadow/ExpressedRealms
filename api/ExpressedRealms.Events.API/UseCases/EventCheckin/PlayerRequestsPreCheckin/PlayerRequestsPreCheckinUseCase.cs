using ExpressedRealms.DB.Models.Checkins.CheckinSetup;
using ExpressedRealms.DB.Models.Checkins.CheckinStageMappingSetup;
using ExpressedRealms.DB.Models.Checkins.CheckinStageSetup;
using ExpressedRealms.Events.API.Discord;
using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.PlayerRequestsPreCheckin;

internal sealed class PlayerRequestsPreCheckinUseCase(
    IEventCheckinRepository checkinRepository,
    IDiscordService discordService,
    IUserContext userContext,
    TimeProvider timeProvider
) : IPlayerRequestsPreCheckinUseCase
{
    public async Task<Result> ExecuteAsync()
    {
        var eventId = await checkinRepository.GetExclusivePreCheckinEventId();
        if (eventId is null)
            return Result.Fail("There are no active events to assign xp to");
        var playerId = await checkinRepository.GetCurrentPlayerId();
        var checkinId = await GetCheckinId(eventId, playerId);

        await checkinRepository.CompleteStage(
            new CheckinStageMapping()
            {
                CreatedAt = timeProvider.GetUtcNow(),
                ApproverUserId = userContext.CurrentUserId(),
                CheckinStageId = CheckinStageEnum.AssignedXpCheck.Value,
                CheckinId = checkinId,
            }
        );

        await checkinRepository.CompleteStage(
            new CheckinStageMapping()
            {
                CreatedAt = timeProvider.GetUtcNow(),
                ApproverUserId = userContext.CurrentUserId(),
                CheckinStageId = CheckinStageEnum.PlayerEarlyCheckin.Value,
                CheckinId = checkinId,
            }
        );

        var seekingCrbMessage = $"A Character has Requested Pre GO Approval";
        await discordService.SendMessageToChannelAsync(
            DiscordChannel.PreCheckinLoadingBay,
            seekingCrbMessage
        );

        return Result.Ok();
    }

    private async Task<int> GetCheckinId(int? eventId, Guid playerId)
    {
        int? checkinId = null;

        var checkin = await checkinRepository.GetCheckinAsync(eventId!.Value, playerId);
        checkinId = checkin?.Id;
        if (checkin is null)
        {
            checkinId = await checkinRepository.CreateCheckinAsync(
                new Checkin() { PlayerId = playerId, EventId = eventId.Value }
            );
        }

        return checkinId!.Value;
    }
}
