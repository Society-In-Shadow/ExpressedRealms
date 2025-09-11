using ExpressedRealms.Blessings.Repository.Blessings;
using ExpressedRealms.Blessings.Repository.CharacterBlessings;
using ExpressedRealms.DB.Models.Blessings.CharacterBlessingMappings;
using ExpressedRealms.Shared;
using ExpressedRealms.UseCases.Shared;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FluentResults;

namespace ExpressedRealms.Blessings.UseCases.CharacterBlessingMappings.Create;

internal sealed class AddBlessingToCharacterUseCase(
    ICharacterBlessingRepository mappingRepository,
    IBlessingRepository blessingRepository,
    AddBlessingToCharacterModelValidator validator,
    CancellationToken cancellationToken
) : IAddBlessingToCharacterUseCase
{
    public async Task<Result<int>> ExecuteAsync(AddBlessingToCharacterModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        // Get Character
        // Check if it's an active character
        // If so, check create status

        // Also need to take into consideration discretionary spending

        // Assuming character creation rules for now
        const int availableExperience = StartingExperience.StartingBlessings;

        var spentXp = await mappingRepository.GetExperienceSpentOnBlessingsForCharacter(
            model.CharacterId
        );
        
        var blessingLevel = await blessingRepository.GetBlessingLevel(
            model.BlessingLevelId
        );

        if (spentXp + blessingLevel.XpCost > availableExperience)
            return Result.Fail(
                new NotEnoughXPFailure(availableExperience - spentXp, blessingLevel.XpCost)
            );

        var mappingId = await mappingRepository.AddCharacterBlessingMapping(
            new CharacterBlessingMapping()
            {
                BlessingLevelId = model.BlessingLevelId,
                CharacterId = model.CharacterId,
                BlessingId = model.BlessingId,
                Notes = model.Notes?.Trim() == string.Empty ? null : model.Notes?.Trim(),
            }
        );

        return Result.Ok(mappingId);
    }
}
