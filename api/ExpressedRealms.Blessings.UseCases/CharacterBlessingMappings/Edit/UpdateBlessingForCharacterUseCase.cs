using ExpressedRealms.Blessings.Repository.Blessings;
using ExpressedRealms.Blessings.Repository.CharacterBlessings;
using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.DB.Characters.xpTables;
using ExpressedRealms.UseCases.Shared;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FluentResults;

namespace ExpressedRealms.Blessings.UseCases.CharacterBlessingMappings.Edit;

internal sealed class UpdateBlessingForCharacterUseCase(
    ICharacterBlessingRepository mappingRepository,
    IBlessingRepository blessingRepository,
    ICharacterRepository characterRepository,
    IXpRepository xpRepository,
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

        var mapping = await mappingRepository.GetCharacterBlessingMappingForEditing(
            model.MappingId
        );

        var characterState = await characterRepository.GetCharacterState(model.CharacterId);

        if (
            mapping.BlessingLevelId != model.BlessingLevelId
            && characterState.IsInCharacterCreation
        )
        {
            var blessing = await blessingRepository.GetBlessingForEditing(mapping.BlessingId);
            var newLevel = await blessingRepository.GetBlessingLevel(model.BlessingLevelId);
            var oldLevel = await blessingRepository.GetBlessingLevel(mapping.BlessingLevelId);

            var xpTypeId = (int)XpSectionTypes.Advantages;
            var oldCost = oldLevel.XpCost;
            var newCost = newLevel.XpCost;
            var name = "Advantages";

            if (blessing.Type.Equals("disadvantage", StringComparison.InvariantCultureIgnoreCase))
            {
                xpTypeId = (int)XpSectionTypes.Disadvantages;
                oldCost = oldLevel.XpGain;
                newCost = newLevel.XpGain;
                name = "Disadvantages";
            }

            var xpInfo = await xpRepository.GetCharacterXpMapping(model.CharacterId, xpTypeId);
            var spentXp = xpInfo.SpentXp;

            spentXp -= oldCost;

            if (spentXp + newCost > xpInfo.SectionCap)
            {
                return Result.Fail(
                    $"You cannot add more than {xpInfo.SectionCap} points of {name}."
                );
            }

            if (blessing.Type.Equals("advantage", StringComparison.InvariantCultureIgnoreCase))
            {
                var availableDiscretionary = await xpRepository.GetAvailableDiscretionary(
                    model.CharacterId
                );

                if (newCost > availableDiscretionary + oldCost)
                {
                    return Result.Fail(
                        new NotEnoughXPFailure(availableDiscretionary, newLevel.XpCost - oldCost)
                    );
                }
            }

            mapping.BlessingLevelId = model.BlessingLevelId;
        }

        mapping.Notes = model.Notes?.Trim() == string.Empty ? null : model.Notes?.Trim();

        await mappingRepository.UpdateMapping(mapping);

        return Result.Ok();
    }
}
