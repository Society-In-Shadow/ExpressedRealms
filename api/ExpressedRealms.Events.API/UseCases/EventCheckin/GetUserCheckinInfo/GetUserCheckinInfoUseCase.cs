using ExpressedRealms.Events.API.Repositories.EventCheckin;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.GetUserCheckinInfo;

internal sealed class GetUserCheckinInfoUseCase(IEventCheckinRepository checkinRepository)
    : IGetUserCheckinInfoUseCase
{
    public async Task<Result<GetUserCheckinInfoReturnModel>> ExecuteAsync()
    {
        var lookupId = await checkinRepository.GetPlayerLookupId();
        var activeEventId = await checkinRepository.GetActiveEventId();

        if (activeEventId == null)
            return Result.Fail("No Active Event Found");

        return Result.Ok(
            new GetUserCheckinInfoReturnModel()
            {
                LookupId = lookupId,
                CheckinStageId = 1,
                EventId = activeEventId.Value,
            }
        );
    }
}
