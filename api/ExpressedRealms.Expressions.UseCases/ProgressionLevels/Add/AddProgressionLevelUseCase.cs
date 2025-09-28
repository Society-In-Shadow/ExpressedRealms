using ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionLevels;
using ExpressedRealms.Expressions.Repository.ProgressionPaths;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.ProgressionLevels.Add;

internal sealed class AddProgressionLevelUseCase(
    IProgressionPathRepository repository,
    AddProgressionLevelModelValidator validator,
    CancellationToken cancellationToken
) : IAddProgressionLevelUseCase
{
    public async Task<Result<int>> ExecuteAsync(AddProgressionLevelModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var returnedId = await repository.CreateProgressionLevel(
            new ProgressionLevel()
            {
                Description = model.Description,
                ProgressionPathId = model.ProgressionId,
                XlLevel = model.XlLevel
            }
        );

        return Result.Ok(returnedId);
    }
}
