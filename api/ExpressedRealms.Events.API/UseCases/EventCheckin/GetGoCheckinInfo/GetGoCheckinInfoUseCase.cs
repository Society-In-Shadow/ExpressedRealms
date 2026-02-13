using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.GetGoCheckinInfo;

internal sealed class GetGoCheckinInfoUseCase(
    IEventCheckinRepository checkinRepository,
    GetGoCheckinInfoModelValidator validator,
    CancellationToken cancellationToken
) : IGetGoCheckinInfoUseCase
{
    public async Task<Result<GetGoCheckinInfoReturnModel>> ExecuteAsync(GetGoCheckinInfoModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var playerName = await checkinRepository.GetUserName(model.LookupId);
        var isFirstTimePlayer = await checkinRepository.IsFirstTimePlayer(model.LookupId);
        var eventId = await checkinRepository.GetActiveEventId();
        var playerId = await checkinRepository.GetPlayerId(model.LookupId);
        var checkin = await checkinRepository.GetCheckinAsync(eventId!.Value, playerId);
        
        return Result.Ok(
            new GetGoCheckinInfoReturnModel()
            {
                Username = playerName,
                IsFirstTimeUser = isFirstTimePlayer,
                AlreadyCheckedIn = checkin is not null,
            }
        );
    }
}
