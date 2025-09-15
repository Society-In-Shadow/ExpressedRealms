using ExpressedRealms.Blessings.Repository.Blessings;
using ExpressedRealms.Blessings.Repository.CharacterBlessings;
using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.DB.Models.Blessings.CharacterBlessingMappings;
using ExpressedRealms.Shared;
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
            Result.Fail("Character must be in character creation to add Advantages / Disadvantages.");
        }

        var xpInfo = await xpRepository.GetCharacterXpMapping(model.CharacterId, 1);
        var spentXp = xpInfo.SpentXp;

        var blessingLevel = await blessingRepository.GetBlessingLevel(model.BlessingLevelId);
        
        if (spentXp + blessingLevel.XpCost > StartingExperience.StartingBlessings)
        {
            return Result.Fail("Cannot add more than 8 points of Advantages / Disadvantages.");
        }
        
        var blessing = await blessingRepository.GetBlessingForEditing(model.BlessingId);
        if (blessing.Type.Equals("advantage", StringComparison.InvariantCultureIgnoreCase))
        {
            var availableDiscretionary = await xpRepository.GetAvailableDiscretionary(model.CharacterId);
            
            if (spentXp + blessingLevel.XpCost > availableDiscretionary)
                return Result.Fail(
                    new NotEnoughXPFailure(availableDiscretionary - spentXp, blessingLevel.XpCost)
                );

            xpInfo.SpentXp = spentXp + blessingLevel.XpCost;
            xpInfo.DiscretionXp = xpInfo.SpentXp;
            xpInfo.TotalCharacterCreationXp = xpInfo.SpentXp;
            xpInfo.LevelXp = 0;
            await xpRepository.UpdateXpInfo(xpInfo);
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
