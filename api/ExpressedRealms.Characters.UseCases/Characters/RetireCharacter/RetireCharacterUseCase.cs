using ExpressedRealms.Characters.Repository;
using ExpressedRealms.DB.Models.Checkins.CheckinStageSetup;
using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.Events.API.UseCases.EventCheckin.ApproveStageAndSendMessages;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.Characters.RetireCharacter;

internal sealed class RetireCharacterUseCase(
    ICharacterRepository repository,
    IEventCheckinRepository checkinRepository,
    IApproveStageAndSendMessageUseCase approveStageAndSendMessageUseCase,
    RetireCharacterModelValidator validator,
    CancellationToken cancellationToken
) : IRetireCharacterUseCase
{
    public async Task<Result> ExecuteAsync(RetireCharacterModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var playerId = await checkinRepository.GetPlayerId(model.LookupId);
        var primaryCharacter = await checkinRepository.GetPrimaryCharacterInformation(playerId);
        var character = await repository.FindCharacterAsync(primaryCharacter!.CharacterId);

        character!.IsPrimaryCharacter = false;
        character.IsRetired = true;
        character.RetiredDate = DateTimeOffset.UtcNow;

        var activeEvent = await checkinRepository.GetActiveEventId();

        if (activeEvent is not null)
        {
            var lookupId = await checkinRepository.GetPlayerLookupId(playerId);
            await approveStageAndSendMessageUseCase.ExecuteAsync(
                new ApproveStageAndSendMessageModel()
                {
                    LookupId = lookupId,
                    StageId = CheckinStageEnum.PlayerNeedsReapproval.Value,
                }
            );
        }

        await repository.EditAsync(character);

        return Result.Ok();
    }
}
