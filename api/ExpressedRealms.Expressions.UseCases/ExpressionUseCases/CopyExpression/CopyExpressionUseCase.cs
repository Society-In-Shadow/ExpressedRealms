using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.ExpressionUseCases.CopyExpression;

internal sealed class CopyExpressionUseCase(
    IExpressionRepository expressionRepository,
    CopyExpressionModelValidator validator,
    CancellationToken cancellationToken
) : ICopyExpressionUseCase
{
    public async Task<Result<int>> ExecuteAsync(CopyExpressionModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var expressionId = await expressionRepository.CopyExpressionAsync(
            model.Id,
            model.ExpressionName
        );

        return Result.Ok(expressionId);
    }
}
