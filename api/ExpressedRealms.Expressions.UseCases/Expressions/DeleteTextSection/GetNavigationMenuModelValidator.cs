using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.Expressions.Repository.ExpressionTextSections;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Expressions.UseCases.Expressions.DeleteTextSection;

[UsedImplicitly]
internal sealed class GetNavigationMenuModelValidator : AbstractValidator<GetNavigationMenuModel>
{
    public GetNavigationMenuModelValidator(
        IExpressionTextSectionRepository textRepository,
        IExpressionRepository expressionRepository
    )
    {
        RuleFor(x => x.ExpressionTypeId)
            .NotEmpty()
            .WithMessage("ExpressionTypeId is required.")
            .MustAsync(async (x, y) => await expressionRepository.ExpressionTypeExists(x))
            .WithMessage("This is not a valid expression.");
    }
}
