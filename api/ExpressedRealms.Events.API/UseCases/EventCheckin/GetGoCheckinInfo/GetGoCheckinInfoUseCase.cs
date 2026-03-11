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

        var playerName = await checkinRepository.GetPlayerName(model.LookupId);
        var isFirstTimePlayer = await checkinRepository.IsFirstTimePlayer(model.LookupId);
        var eventId = await checkinRepository.GetActiveEventId();
        var playerId = await checkinRepository.GetPlayerId(model.LookupId);
        var checkin = await checkinRepository.GetCheckinAsync(eventId!.Value, playerId);

        // If user has age group, and they are adult, automatically push them to next step
        // If user has age group, and they are 13-17, reask - they might have turned 18
        // If the user doesn't have an age group ask

        // If user is under 13, block checkin functionality

        // This should return
        // - User Info
        //   - User Name
        //   - Age Group
        // - Next Stage That needs to be completed

        return Result.Ok(
            new GetGoCheckinInfoReturnModel()
            {
                Username = playerName, // Displayed to make sure the QR Code got the right person
                IsFirstTimeUser = isFirstTimePlayer, // Only care about this for XP checkin purposes
                AlreadyCheckedIn = checkin is not null, // This is to lock changing the age after intial checkin
            }
        );
    }
}
