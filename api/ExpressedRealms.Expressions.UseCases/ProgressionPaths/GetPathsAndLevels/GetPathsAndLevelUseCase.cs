using ExpressedRealms.Expressions.Repository.ProgressionPaths;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.ProgressionPaths.GetPathsAndLevels;

internal sealed class GetPathsAndLevelsUseCase(
    IProgressionPathRepository repository,
    GetPathsAndLevelsModelValidator validator,
    CancellationToken cancellationToken
) : IGetPathsAndLevelsUseCase
{
    public async Task<Result<List<ProgressionPathReturnModel>>> ExecuteAsync(
        GetPathsAndLevelsModel model
    )
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var progressionPath = await repository.GetProgressionPathsAndLevelsForExpression(
            model.ExpressionId
        );

        return Result.Ok(
            progressionPath
                .Select(x => new ProgressionPathReturnModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Levels = x
                        .ProgressionLevels.Select(x => new ProgressionLevelReturnModel()
                        {
                            Id = x.Id,
                            Description = x.Description,
                            XlLevel = x.XlLevel,
                            ModifierGroupId = x.StatModifierGroupId,
                        })
                        .ToList(),
                })
                .ToList()
        );
    }
}
