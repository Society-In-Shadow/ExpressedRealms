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
    public async Task<Result> ExecuteAsync(EditBlessingLevelModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var level = await blessingRepository.GetBlessingLevelForEditing(
            model.BlessingId,
            model.LevelId
        );

        var updatedDescription = level.Description != model.Description;
        level.XpCost = model.XpCost;
        level.XpGain = model.XpGain;
        level.Level = model.Level;
        level.Description = model.Description;

        await blessingRepository.EditBlessingLevelAsync(level);

        if (updatedDescription)
        {
            var blessing = await blessingRepository.GetBlessingForEditing(model.BlessingId);
            var levelDescriptions = await blessingRepository.GetBlessingLevelsForSearchUpdate(
                model.BlessingId
            );
            blessing.ProcessedPlainText =
                blessing.Description.ToPlainText()
                + string.Join("", levelDescriptions).ToPlainText();

            await blessingRepository.EditBlessingAsync(blessing);
        }

        return Result.Ok();
    }
}
