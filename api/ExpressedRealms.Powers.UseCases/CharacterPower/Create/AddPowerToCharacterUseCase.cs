using ExpressedRealms.DB.Models.Powers.CharacterPowerMappingSetup;
using ExpressedRealms.Powers.Repository.CharacterPower;
using ExpressedRealms.Powers.Repository.Powers;
using ExpressedRealms.Shared;
using ExpressedRealms.UseCases.Shared;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FluentResults;

namespace ExpressedRealms.Powers.UseCases.CharacterPower.Create;

internal sealed class AddPowerToCharacterUseCase(
    ICharacterPowerRepository mappingRepository,
    IPowerRepository powerRepository,
    AddPowerToCharacterModelValidator validator,
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

        // Get Character
        // Check if it's an active character
        // If so, check create status

        // Also need to take into consideration discretionary spending

        // Assuming character creation rules for now
        const int availableExperience = StartingExperience.StartingPowers;

        var spentXp = await mappingRepository.GetExperienceSpentOnPowersForCharacter(
            model.CharacterId
        );

        var powerLevel = await powerRepository.GetPowerLevelForPower(model.PowerId);

        if (spentXp + powerLevel.Xp > availableExperience)
            return Result.Fail(
                new NotEnoughXPFailure(availableExperience - spentXp, powerLevel.Xp)
            );

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
