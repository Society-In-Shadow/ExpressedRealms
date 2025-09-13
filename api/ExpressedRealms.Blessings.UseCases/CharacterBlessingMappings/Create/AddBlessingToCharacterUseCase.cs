using ExpressedRealms.Blessings.Repository.Blessings;
using ExpressedRealms.Blessings.Repository.CharacterBlessings;
using ExpressedRealms.DB.Models.Blessings.CharacterBlessingMappings;
using ExpressedRealms.Shared;
using ExpressedRealms.UseCases.Shared;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FluentResults;

namespace ExpressedRealms.Blessings.UseCases.CharacterBlessingMappings.Create;

internal sealed class AddBlessingToCharacterUseCase(
    ICharacterBlessingRepository mappingRepository,
    IBlessingRepository blessingRepository,
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

        // Covers both advantage cap (8pts) and disadvantage cap (8pts)
        const int availableExperience = StartingExperience.StartingBlessings;

        var spentXp = await mappingRepository.GetSpentXpForBlessingType(model.CharacterId, model.BlessingId);

        var blessingLevel = await blessingRepository.GetBlessingLevel(model.BlessingLevelId);

        if (spentXp + blessingLevel.XpCost > availableExperience)
            return Result.Fail(
                new NotEnoughXPFailure(availableExperience - spentXp, blessingLevel.XpCost)
            );

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
