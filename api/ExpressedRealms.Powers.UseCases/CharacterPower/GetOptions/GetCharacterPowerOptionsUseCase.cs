using ExpressedRealms.Powers.Repository.CharacterPower;
using ExpressedRealms.Powers.Repository.Powers;
using ExpressedRealms.Shared;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Powers.UseCases.CharacterPower.GetOptions;

public class GetCharacterPowerOptionsUseCase(
    IPowerRepository powerRepository,
    ICharacterPowerRepository mappingRepository,
    GetCharacterPowerOptionsModelValidator validator,
    CancellationToken cancellationToken
) : IGetCharacterPowerOptionsUseCase
{
    public async Task<Result<GetCharacterPowerOptionsReturnModel>> ExecuteAsync(
        GetCharacterPowerOptionsModel model
    )
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var powerExperienceCost = await powerRepository.GetPowerExperienceCost(model.PowerId);
        var spentXp = await mappingRepository.GetExperienceSpentOnPowersForCharacter(
            model.CharacterId
        );

        return Result.Ok(
            new GetCharacterPowerOptionsReturnModel()
            {
                AvailableExperience = StartingExperience.StartingPowers - spentXp,
                PowerLevelExperience = powerExperienceCost,
            }
        );
    }
}
