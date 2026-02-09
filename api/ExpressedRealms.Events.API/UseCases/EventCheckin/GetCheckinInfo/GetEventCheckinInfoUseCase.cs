using ExpressedRealms.Events.API.Repositories.EventCheckin;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.GetCheckinInfo;

internal sealed class GetEventCheckinInfoUseCase(
    IEventCheckinRepository checkinRepository
) : IGetEventCheckinInfoUseCase
{
    public async Task<Result<CheckinInfoReturnModel>> ExecuteAsync()
    {
        var lookupId = await checkinRepository.GetPlayerLookupId();
        var activeEventId = await checkinRepository.GetActiveEventId();
        
        if(activeEventId == null)
            return Result.Fail("No active event found");

        return Result.Ok(new CheckinInfoReturnModel()
            {
                LookupId = lookupId,
                CheckinStageId = 1,
                EventId = activeEventId.Value
            }
        );
    }
}
