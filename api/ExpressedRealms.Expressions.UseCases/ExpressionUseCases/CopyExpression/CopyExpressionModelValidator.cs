using ExpressedRealms.Expressions.Repository.Expressions;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Expressions.UseCases.ExpressionUseCases.CopyExpression;

[UsedImplicitly]
internal sealed class CopyExpressionModelValidator : AbstractValidator<CopyExpressionModel>
{
    public CopyExpressionModelValidator(IExpressionRepository repository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .MustAsync(async (x, y) => await repository.ExpressionExists(x) is not null)
            .WithMessage("Expression does not exist.");

        RuleFor(x => x.ExpressionName).NotEmpty().MaximumLength(250);
    }
}
