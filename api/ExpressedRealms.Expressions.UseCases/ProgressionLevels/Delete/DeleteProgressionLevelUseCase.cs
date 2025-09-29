using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.Expressions.Repository.ProgressionPaths;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.ProgressionLevels.Delete;

internal sealed class DeleteProgressionLevelUseCase(
    IProgressionPathRepository repository,
    DeleteProgressionLevelModelValidator validator,
    CancellationToken cancellationToken
) : IDeleteProgressionLevelUseCase
{
    public async Task<Result> ExecuteAsync(DeleteProgressionLevelModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var level = await repository.GetProgressionLevelForEditing(model.ProgressionLevelId);

        level.SoftDelete();

        await repository.SaveProgressionLevelChanges(level);

        return Result.Ok();
    }
}
