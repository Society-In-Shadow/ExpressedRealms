using ExpressedRealms.Blessings.Repository.Blessings;
using ExpressedRealms.Blessings.Repository.CharacterBlessings;
using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Xp;
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

        if (mapping.BlessingLevelId != model.BlessingLevelId)
        {
            var characterState = await characterRepository.GetCharacterState(model.CharacterId);

            if (!characterState.IsInCharacterCreation)
            {
                return Result.Fail("Character must be in character creation to modify Advantages / Disadvantages.");
            }
            
            var xpInfo = await xpRepository.GetCharacterXpMapping(model.CharacterId, 1);
            var spentXp = xpInfo.SpentXp;

            var newLevel = await blessingRepository.GetBlessingLevel(model.BlessingLevelId);
            var oldLevel = await blessingRepository.GetBlessingLevel(mapping.BlessingLevelId);

            var gainingXp = newLevel.XpCost < oldLevel.XpCost;
            if (gainingXp)
            {
                spentXp -= oldLevel.XpCost;
            }

            if (spentXp + newLevel.XpCost > xpInfo.SectionCap)
            {
                return Result.Fail("Cannot add more than 8 points of Advantages / Disadvantages.");
            }
            
            var blessing = await blessingRepository.GetBlessingForEditing(mapping.BlessingId);
            if (blessing.Type.Equals("advantage", StringComparison.InvariantCultureIgnoreCase))
            {
                var availableDiscretionary = await xpRepository.GetAvailableDiscretionary(model.CharacterId);

                if (spentXp + newLevel.XpCost > availableDiscretionary)
                {
                    return Result.Fail(
                        new NotEnoughXPFailure(availableDiscretionary - spentXp, newLevel.XpCost)
                    );
                }

                xpInfo.SpentXp = spentXp + newLevel.XpCost;
                xpInfo.DiscretionXp = xpInfo.SpentXp;
                xpInfo.TotalCharacterCreationXp = xpInfo.SpentXp;
                xpInfo.LevelXp = 0;
                await xpRepository.UpdateXpInfo(xpInfo);
            }

            mapping.BlessingLevelId = model.BlessingLevelId;
        }

        mapping.Notes = model.Notes?.Trim() == string.Empty ? null : model.Notes?.Trim();

        await mappingRepository.UpdateMapping(mapping);

        return Result.Ok();
    }
}
