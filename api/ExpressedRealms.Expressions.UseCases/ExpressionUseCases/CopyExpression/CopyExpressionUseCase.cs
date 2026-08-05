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

        var isDuplicateName = await expressionRepository.HasDuplicateName(model.Name);
        if (isDuplicateName)
            return ValidationHelper.AddSingleValidationFailure(
                nameof(model.Name),
                "This is a duplicate name."
            );

        var expressionId = await expressionRepository.CopyExpressionAsync(model.Id, model.Name);

        return Result.Ok(expressionId);
    }
}
