using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.Expressions.Repository.ProgressionPaths;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.ProgressionPaths.Delete;

internal sealed class DeleteProgressionPathUseCase(
    IProgressionPathRepository repository,
    DeleteProgressionPathModelValidator validator,
    CancellationToken cancellationToken
) : IDeleteProgressionPathUseCase
{
    public async Task<Result> ExecuteAsync(DeleteProgressionPathModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var progressionPath = await repository.GetProgressionPathForEditing(model.Id);

        progressionPath.SoftDelete();

        await repository.SaveProgressionPathChanges(progressionPath);
        
        return Result.Ok();
    }
}
