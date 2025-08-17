using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.Expressions.Repository.ExpressionTextSections;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Expressions.UseCases.ExpressionTextSections.GetExpressionBooklet;

[UsedImplicitly]
internal sealed class GetExpressionBookletModelValidator
    : AbstractValidator<GetExpressionBookletModel>
{
    public GetExpressionBookletModelValidator(
        IExpressionTextSectionRepository textRepository,
        IExpressionRepository expressionRepository
    )
    {
        RuleFor(x => x.ExpressionId)
            .NotEmpty()
            .WithMessage("ExpressionId is required.")
            .MustAsync(async (x, y) => await expressionRepository.ExpressionExists(x) != null)
            .WithErrorCode("NotFound")
            .WithMessage("This is not a valid expression.");
    }
}
