using ExpressedRealms.Expressions.Repository.ProgressionPaths;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.ProgressionPaths.Edit;

internal sealed class EditProgressionPathUseCase(
    IProgressionPathRepository repository,
    EditProgressionPathModelValidator validator,
    CancellationToken cancellationToken
) : IEditProgressionPathUseCase
{
    public async Task<Result> ExecuteAsync(EditProgressionPathModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var progressionPath = await repository.GetProgressionPathForEditing(model.Id);

        progressionPath.Name = model.Name;
        progressionPath.Description = model.Description;

        await repository.SaveProgressionPathChanges(progressionPath);

        return Result.Ok();
    }
}
