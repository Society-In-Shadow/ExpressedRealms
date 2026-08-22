using ExpressedRealms.DB.Models.Characters.CharacterStorage.CharacterStorageModels;
using ExpressedRealms.DB.Models.Checkins.CheckinStageSetup;
using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.Events.API.UseCases.EventCheckin.ApproveStageAndSendMessages;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.UpdateCharacterStorageInfo;

internal sealed class UpdateCharacterStorageInfoUseCase(
    IEventCheckinRepository checkinRepository,
    TimeProvider timeProvider,
    IApproveStageAndSendMessageUseCase approveStageAndSendMessageUseCase,
    UpdateCharacterStorageInfoModelValidator validator,
    CancellationToken cancellationToken
) : IUpdateCharacterStorageInfoUseCase
{
    public async Task<Result> ExecuteAsync(UpdateCharacterStorageInfoModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var playerId = await checkinRepository.GetPlayerId(model.LookupId);
        var eventId = await checkinRepository.GetActiveEventId();

        if (eventId is null)
        {
            return Result.Fail("You need an active event to update character storage.");
        }

        var characterStorageInfo = await checkinRepository.GetCharacterStorageInfo(
            playerId,
            eventId.Value
        );
        if (characterStorageInfo is not null)
        {
            return Result.Fail("Character Storage has already been tracked.");
        }

        await checkinRepository.AddCharacterStorageInfo(
            new CharacterStorageInfo()
            {
                PlayerId = playerId,
                EventId = eventId.Value,
                CollectorPlayerId = await checkinRepository.GetCurrentPlayerId(),
                Timestamp = timeProvider.GetUtcNow(),
                OptedIn = model.OptedIn,
                Amount = 20,
            }
        );

        await approveStageAndSendMessageUseCase.ExecuteAsync(
            new() { LookupId = model.LookupId, StageId = CheckinStageEnum.CharacterStorageQuestion }
        );

        return Result.Ok();
    }
}
