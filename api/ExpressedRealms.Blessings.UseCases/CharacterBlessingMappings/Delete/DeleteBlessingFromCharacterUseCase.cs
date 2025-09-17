using ExpressedRealms.Blessings.Repository.Blessings;
using ExpressedRealms.Blessings.Repository.CharacterBlessings;
using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.DB.Characters.xpTables;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Blessings.UseCases.CharacterBlessingMappings.Delete;

internal sealed class DeleteBlessingFromCharacterUseCase(
    ICharacterBlessingRepository mappingRepository,
    DeleteBlessingFromCharacterModelValidator validator,
    ICharacterRepository characterRepository,
    IBlessingRepository blessingRepository,
    IXpRepository xpRepository,
    CancellationToken cancellationToken
) : IDeleteBlessingFromCharacterUseCase
{
    public async Task<Result> ExecuteAsync(DeleteBlessingFromCharacterModel model)
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
            return Result.Fail("You cannot delete Advantages or Disadvantages outside of character creation.");
        }
        
        var mapping = await mappingRepository.GetCharacterBlessingMappingForEditing(
            model.MappingId
        );

        mapping.SoftDelete();

        await mappingRepository.UpdateMapping(mapping);

        var blessingLevel = await blessingRepository.GetBlessingLevel(mapping.BlessingLevelId);
        var xpTypeId = (int)XpSectionTypeEnum.Advantages;
        var blessing = await blessingRepository.GetBlessingForEditing(mapping.BlessingId);
        
        var cost = blessingLevel.XpCost;
        if (blessing.Type.Equals("disadvantage", StringComparison.InvariantCultureIgnoreCase))
        {
            xpTypeId = (int)XpSectionTypeEnum.Disadvantages;
            cost = blessingLevel.XpGain;
        }
        var xpInfo = await xpRepository.GetCharacterXpMapping(model.CharacterId, xpTypeId);

        xpInfo.SpentXp -= cost;
        xpInfo.DiscretionXp = xpInfo.SpentXp;
        xpInfo.TotalCharacterCreationXp = xpInfo.SpentXp;
        xpInfo.LevelXp = 0;
        
        await xpRepository.UpdateXpInfo(xpInfo);

        return Result.Ok();
    }
}
