using ExpressedRealms.Characters.Repository;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.Expressions.Repository.CharacterFactions;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.CharacterFactionMapping.LeaveFaction;

internal sealed class LeaveFactionUseCase(
    ICharacterRepository characterRepository,
    ICharacterFactionRepository characterFactionRepository,
    LeaveFactionModelValidator validator,
    CancellationToken cancellationToken
) : ILeaveFactionUseCase
{
    public async Task<Result<int>> ExecuteAsync(LeaveFactionModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var character = await characterRepository.FindCharacterAsync(model.CharacterId);
        if (character is null)
            return ValidationHelper.AddSingleValidationFailure(
                nameof(model.CharacterId),
                "Character Id does not exist."
            );

        var factionMappings = await characterFactionRepository.GetFactionLevelsForBulkEditing(
            model.CharacterId
        );

        foreach (var factionMapping in factionMappings)
        {
            factionMapping.SoftDelete();
        }
        
        await characterFactionRepository.BulkEditCharacterFactionAsync(factionMappings);

        return Result.Ok();
    }
}
