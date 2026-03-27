using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.Characters.CopyCharacter;

internal sealed class CopyCharacterUseCase(
    ICharacterRepository characterRepository,
    IUserContext userContext,
    CopyCharacterModelValidator validator,
    CancellationToken cancellationToken
) : ICopyCharacterUseCase
{
    public async Task<Result<int>> ExecuteAsync(CopyCharacterModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var playerId = await characterRepository.GetPlayerId(userContext.CurrentUserId());
        var characterId = await characterRepository.CopyCharacterAsync(
            model.Id,
            playerId,
            model.CharacterName
        );

        return Result.Ok(characterId);
    }
}
