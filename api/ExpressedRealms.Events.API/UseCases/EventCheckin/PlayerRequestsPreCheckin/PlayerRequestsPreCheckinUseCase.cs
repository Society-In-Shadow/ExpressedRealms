using ExpressedRealms.DB.Models.Checkins.CheckinSetup;
using ExpressedRealms.Events.API.Discord;
using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.PlayerRequestsPreCheckin;

internal sealed class PlayerRequestsPreCheckinUseCase(
    IEventCheckinRepository checkinRepository,
    IDiscordService discordService,
    PlayerRequestsPreCheckinModelValidator validator,
    CancellationToken cancellationToken
) : IPlayerRequestsPreCheckinUseCase
{
    public async Task<Result> ExecuteAsync(PlayerRequestsPreCheckinModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var eventId = await checkinRepository.GetExclusivePreCheckinEventId();
        var playerId = await checkinRepository.GetCurrentPlayerId();
        var checkinId = await GetCheckinId(eventId, playerId);
        
        // TODO: Approve Early Checkin Approval Step
        
        var seekingCrbMessage = $"A Character was put into Early Checkin Queue";
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
