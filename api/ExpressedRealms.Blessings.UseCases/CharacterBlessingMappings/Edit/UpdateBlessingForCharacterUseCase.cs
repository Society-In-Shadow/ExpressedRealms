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
    public async Task<Result> ExecuteAsync(UpdateBlessingForCharacterModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        // Also need to take into consideration discretionary spending
        var mapping = await mappingRepository.GetCharacterBlessingMappingForEditing(
            model.MappingId
        );

        if (mapping.BlessingLevelId != model.BlessingLevelId)
        {
            // Covers both advantage cap (8pts) and disadvantage cap (8pts)
            const int availableExperience = StartingExperience.StartingBlessings;

            var spentXp = await mappingRepository.GetSpentXpForBlessingType(
                model.CharacterId,
                mapping.BlessingId
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

            mapping.BlessingLevelId = model.BlessingLevelId;
        }

        mapping.Notes = model.Notes?.Trim() == string.Empty ? null : model.Notes?.Trim();

        await mappingRepository.UpdateMapping(mapping);

        return Result.Ok();
    }
}
