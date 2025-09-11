using ExpressedRealms.Blessings.Repository.Blessings;
using ExpressedRealms.Blessings.Repository.CharacterBlessings;
using ExpressedRealms.Shared;
using ExpressedRealms.UseCases.Shared;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FluentResults;

namespace ExpressedRealms.Blessings.UseCases.CharacterBlessingMappings.Edit;

internal sealed class UpdateBlessingForCharacterUseCase(
    ICharacterBlessingRepository mappingRepository,
    IBlessingRepository blessingRepository,
    UpdateBlessingForCharacterModelValidator validator,
    CancellationToken cancellationToken
) : IUpdateBlessingForCharacterUseCase
{
    public async Task<Result<int>> ExecuteAsync(UpdateBlessingForCharacterModel model)
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
        var mapping = await mappingRepository.GetCharacterBlessingMappingForEditing(
            model.MappingId
        );

        if (mapping.BlessingLevelId != model.BlessingLevelId)
        {
            // Assuming character creation rules for now
            const int availableExperience = StartingExperience.StartingBlessings;

            var spentXp = await mappingRepository.GetExperienceSpentOnBlessingsForCharacter(
                model.CharacterId
            );

            var newLevel = await blessingRepository.GetBlessingLevel(model.BlessingLevelId);
            var oldLevel = await blessingRepository.GetBlessingLevel(mapping.BlessingLevelId);

            var gainingXp = newLevel.XpCost < oldLevel.XpCost;
            if (gainingXp)
            {
                spentXp -= oldLevel.XpCost;
            }

            if (spentXp + newLevel.XpCost > availableExperience)
            {
                return Result.Fail(
                    new NotEnoughXPFailure(availableExperience - spentXp, newLevel.XpCost)
                );
            }
        }

        mapping.Notes = model.Notes?.Trim() == string.Empty ? null : model.Notes?.Trim();

        await mappingRepository.UpdateMapping(mapping);

        return Result.Ok();
    }
}
