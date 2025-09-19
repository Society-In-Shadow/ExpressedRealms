using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.DB.Characters.xpTables;
using ExpressedRealms.DB.Models.Powers.CharacterPowerMappingSetup;
using ExpressedRealms.Powers.Repository.CharacterPower;
using ExpressedRealms.Powers.Repository.Powers;
using ExpressedRealms.UseCases.Shared;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FluentResults;

namespace ExpressedRealms.Powers.UseCases.CharacterPower.Create;

internal sealed class AddPowerToCharacterUseCase(
    ICharacterPowerRepository mappingRepository,
    IPowerRepository powerRepository,
    AddPowerToCharacterModelValidator validator,
    IXpRepository xpRepository,
    CancellationToken cancellationToken
) : IAddPowerToCharacterUseCase
{
    public async Task<Result<int>> ExecuteAsync(AddPowerToCharacterModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var xpInfo = await xpRepository.GetAvailableXpForSection(
            model.CharacterId,
            XpSectionTypeEnum.Powers
        );

        var spentXp = xpInfo.SpentXp;
        var powerLevel = await powerRepository.GetPowerLevelForPower(model.PowerId);

        if (spentXp + powerLevel.Xp > xpInfo.AvailableXp)
            return Result.Fail(new NotEnoughXPFailure(xpInfo.AvailableXp - spentXp, powerLevel.Xp));

        var mappingId = await mappingRepository.AddCharacterPowerMapping(
            new CharacterPowerMapping()
            {
                PowerLevelId = powerLevel.Id,
                CharacterId = model.CharacterId,
                PowerId = model.PowerId,
                Notes = model.Notes?.Trim() == string.Empty ? null : model.Notes?.Trim(),
            }
        );

        return Result.Ok(mappingId);
    }
}
