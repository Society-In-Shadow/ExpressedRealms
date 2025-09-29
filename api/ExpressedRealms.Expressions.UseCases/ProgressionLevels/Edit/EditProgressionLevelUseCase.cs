using ExpressedRealms.Expressions.Repository.ProgressionPaths;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.ProgressionLevels.Edit;

internal sealed class EditProgressionLevelUseCase(
    IProgressionPathRepository repository,
    EditProgressionLevelModelValidator validator,
    CancellationToken cancellationToken
) : IEditProgressionLevelUseCase
{
    public async Task<Result> ExecuteAsync(EditProgressionLevelModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var level = await repository.GetProgressionLevelForEditing(model.ProgressionLevelId);

        level.Description = model.Description;
        level.XlLevel = model.XlLevel;

        await repository.SaveProgressionLevelChanges(level);

        return Result.Ok();
    }
}
