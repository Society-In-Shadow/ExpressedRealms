using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.Powers.UseCases.GetPowerCardReport;
using FluentValidation;

namespace ExpressedRealms.Powers.UseCases.GetPowerBookletReport;

public class GetPowerBookletReportUseCaseModelValidator
    : AbstractValidator<GetPowerCardReportUseCaseModel>
{
    public GetPowerBookletReportUseCaseModelValidator(IExpressionRepository expressionRepository)
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
