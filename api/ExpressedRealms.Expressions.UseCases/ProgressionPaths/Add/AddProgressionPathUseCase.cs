using ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionPaths;
using ExpressedRealms.Expressions.Repository.ProgressionPaths;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.ProgressionPaths.Add;

internal sealed class AddProgressionPathUseCase(
    IProgressionPathRepository repository,
    AddProgressionPathModelValidator validator,
    CancellationToken cancellationToken
) : IAddProgressionPathUseCase
{
    public async Task<Result<int>> ExecuteAsync(AddProgressionPathModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var returnedId = await repository.CreateProgressionPath(
            new ProgressionPath()
            {
                Name = model.Name,
                Description = model.Description,
                ExpressionId = model.ExpressionId
            }
        );

        return Result.Ok(returnedId);
    }
}