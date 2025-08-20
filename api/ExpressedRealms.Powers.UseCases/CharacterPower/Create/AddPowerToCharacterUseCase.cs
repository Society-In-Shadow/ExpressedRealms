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
        const int availableExperience = 20;

        var spentXp = await mappingRepository.GetExperienceSpentOnPowersForCharacter(
            model.CharacterId
        );

        var newPowerExperience = await powerRepository.GetPowerLevelExperience(model.PowerLevelId);

        if (spentXp + newPowerExperience > availableExperience)
            return Result.Fail(
                new NotEnoughXPFailure(availableExperience - spentXp, newPowerExperience)
            );

        var mappingId = await mappingRepository.AddCharacterPowerMapping(
            new CharacterPowerMapping()
            {
                PowerLevelId = model.PowerLevelId,
                CharacterId = model.CharacterId,
                PowerId = model.PowerId,
                Notes = model.Notes?.Trim() == string.Empty ? null : model.Notes?.Trim(),
            }
        );

        return Result.Ok(mappingId);
    }
}
