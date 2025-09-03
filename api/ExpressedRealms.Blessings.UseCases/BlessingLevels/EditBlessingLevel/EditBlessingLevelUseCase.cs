using ExpressedRealms.Blessings.Repository.Blessings;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Blessings.UseCases.BlessingLevels.EditBlessingLevel;

internal sealed class EditBlessingLevelUseCase(
    IBlessingRepository blessingRepository,
    EditBlessingLevelModelValidator validator,
    CancellationToken cancellationToken
) : IEditBlessingLevelUseCase
{
    public async Task<Result<int>> ExecuteAsync(EditBlessingLevelModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var level = await blessingRepository.GetBlessingLevelForEditing(model.LevelId);

        level.BlessingId = model.BlessingId;
        level.XpCost = model.XpCost;
        level.XpGain = model.XpGain;
        level.Level = model.Level;
        level.Description = model.Description;

        await blessingRepository.EditBlessingLevelAsync(level);

        return Result.Ok();
    }
}