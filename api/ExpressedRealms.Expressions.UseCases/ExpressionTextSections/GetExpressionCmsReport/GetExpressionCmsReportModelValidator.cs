using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.Expressions.Repository.ExpressionTextSections;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Expressions.UseCases.ExpressionTextSections.GetExpressionCmsReport;

[UsedImplicitly]
internal sealed class GetExpressionCmsReportModelValidator : AbstractValidator<GetExpressionCmsReportModel>
{
    public GetExpressionCmsReportModelValidator(
        IExpressionTextSectionRepository textRepository,
        IExpressionRepository expressionRepository
    )
    {
        RuleFor(x => x.ExpressionId)
            .NotEmpty()
            .WithMessage("ExpressionId is required.")
            .MustAsync(
                async (x, y) => await expressionRepository.GetExpressionForDeletion(x) != null
            )
            .WithErrorCode("NotFound")
            .WithMessage("This is not a valid expression.")
            .DependentRules(() =>
            {
                RuleFor(x => x.ExpressionId)
                    .MustAsync(
                        async (x, y) =>
                            !(await expressionRepository.GetExpressionForDeletion(x))!.IsDeleted
                    )
                    .WithErrorCode("AlreadyDeleted")
                    .WithMessage("This expression has been deleted.");
            });

    }
}
