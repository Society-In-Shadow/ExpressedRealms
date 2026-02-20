using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.Characters.RetireCharacter;

internal sealed class RetireCharacterUseCase(
    ICharacterRepository repository,
    IEventCheckinRepository checkinRepository,
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

        await repository.EditAsync(character);

        return Result.Ok();
    }
}
