using ExpressedRealms.Authentication.PermissionCollection;
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

        if (!userContext.CurrentUserHasPermission(Permissions.Archetypes.Create) && model.IsArchetype)
        {
            return Result.Fail("You do not have permission to create an archetype character.");
        }

        Guid playerId;
        if (userContext.CurrentUserHasPermission(Permissions.Archetypes.Create) && model.IsArchetype)
        {
            playerId = await characterRepository.GetArchetypePlayerId();
        }
        else
        {
            playerId = await characterRepository.GetPlayerId(userContext.CurrentUserId());
        }
        
        var characterId = await characterRepository.CopyCharacterAsync(
            model.Id,
            playerId,
            model.CharacterName
        );

        return Result.Ok(characterId);
    }
}
