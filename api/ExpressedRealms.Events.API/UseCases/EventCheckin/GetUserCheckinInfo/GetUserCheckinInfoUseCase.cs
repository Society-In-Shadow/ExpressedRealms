using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.Events.API.Repositories.EventCheckin.Dtos;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.GetUserCheckinInfo;

internal sealed class GetUserCheckinInfoUseCase(IEventCheckinRepository checkinRepository)
    : IGetUserCheckinInfoUseCase
{
    public async Task<Result<GetUserCheckinInfoReturnModel>> ExecuteAsync()
    {
        var lookupId = await checkinRepository.GetPlayerLookupId();
        var playerId = await checkinRepository.GetCurrentPlayerId();

        var activeEvent = await checkinRepository.GetActiveEventInfoOrDefaultAsync();

        if (activeEvent == null)
            return Result.Fail("No Active Event Found");

        var checkin = await checkinRepository.GetCheckinAsync(activeEvent.Id, playerId);
        BasicInfo? currentStage = null;
        if (checkin is not null)
        {
            currentStage = await checkinRepository.GetCurrentStage(checkin.Id);
        }

        return Result.Ok(
            new GetUserCheckinInfoReturnModel()
            {
                LookupId = lookupId,
                CheckinStage = currentStage,
                Event = new ActiveEvent() { Id = activeEvent.Id, Name = activeEvent.Name },
            }
        );
    }
}
