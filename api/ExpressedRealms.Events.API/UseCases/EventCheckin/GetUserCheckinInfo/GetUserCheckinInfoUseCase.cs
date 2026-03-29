using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.Events.API.Repositories.EventCheckin.Dtos;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.GetUserCheckinInfo;

internal sealed class GetUserCheckinInfoUseCase(IEventCheckinRepository checkinRepository)
    : IGetUserCheckinInfoUseCase
{
    public async Task<Result<GetUserCheckinInfoReturnModel>> ExecuteAsync()
    {
        var playerInfo = await checkinRepository.GetPlayerInfoForPlayerCheckinPage();
        if (string.IsNullOrWhiteSpace(playerInfo.EventName))
        {
            return Result.Fail("No Active Event Found");
        }
        
        BasicInfo? currentStage = null;
        if (playerInfo.CheckinId is not null)
        {
            currentStage = await checkinRepository.GetCurrentStage(playerInfo.CheckinId.Value);
        }

        return Result.Ok(
            new GetUserCheckinInfoReturnModel()
            {
                LookupId = playerInfo.LookupId,
                SendPickupCrbEmail = playerInfo.SendPickupCrbEmail,
                CheckinStage = currentStage,
                EventName = playerInfo.EventName,
            }
        );
    }
}
