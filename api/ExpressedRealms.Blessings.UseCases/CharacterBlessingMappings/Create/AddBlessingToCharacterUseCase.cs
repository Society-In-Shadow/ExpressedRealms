using ExpressedRealms.Blessings.Repository.Blessings;
using ExpressedRealms.Blessings.Repository.CharacterBlessings;
using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.DB.Characters.xpTables;
using ExpressedRealms.DB.Models.Blessings.CharacterBlessingMappings;
using ExpressedRealms.UseCases.Shared;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FluentResults;

namespace ExpressedRealms.Blessings.UseCases.CharacterBlessingMappings.Create;

internal sealed class AddBlessingToCharacterUseCase(
    ICharacterBlessingRepository mappingRepository,
    IBlessingRepository blessingRepository,
    ICharacterRepository characterRepository,
    IXpRepository xpRepository,
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

        var characterState = await characterRepository.GetCharacterState(model.CharacterId);

        if (!characterState.IsInCharacterCreation)
        {
            return Result.Fail(
                "You cannot add Advantages or Disadvantages outside of character creation."
            );
        }

        var blessing = await blessingRepository.GetBlessingForEditing(model.BlessingId);
        var blessingLevel = await blessingRepository.GetBlessingLevel(model.BlessingLevelId);

        var xpTypeId = (int)XpSectionTypeEnum.Advantages;
        var cost = blessingLevel.XpCost;
        var name = "Advantages";

        if (blessing.Type.Equals("disadvantage", StringComparison.InvariantCultureIgnoreCase))
        {
            xpTypeId = (int)XpSectionTypeEnum.Disadvantages;
            cost = blessingLevel.XpGain;
            name = "Disadvantages";
        }

        var xpInfo = await xpRepository.GetCharacterXpMapping(model.CharacterId, xpTypeId);
        var spentXp = xpInfo.SpentXp;

        if (spentXp + cost > xpInfo.SectionCap)
        {
            return Result.Fail($"You cannot add more than {xpInfo.SectionCap} points of {name}.");
        }

        if (blessing.Type.Equals("advantage", StringComparison.InvariantCultureIgnoreCase))
        {
            var availableDiscretionary = await xpRepository.GetAvailableDiscretionary(
                model.CharacterId
            );

            if (spentXp + blessingLevel.XpCost > availableDiscretionary)
                return Result.Fail(
                    new NotEnoughXPFailure(availableDiscretionary, blessingLevel.XpCost)
                );
        }

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
