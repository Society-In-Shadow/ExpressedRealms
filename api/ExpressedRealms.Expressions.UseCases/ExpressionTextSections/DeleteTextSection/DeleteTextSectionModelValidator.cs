using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.Expressions.Repository.ExpressionTextSections;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Expressions.UseCases.ExpressionTextSections.DeleteTextSection;

[UsedImplicitly]
internal sealed class DeleteTextSectionModelValidator : AbstractValidator<DeleteTextSectionModel>
{
    public DeleteTextSectionModelValidator(
        IExpressionTextSectionRepository textRepository,
        IExpressionRepository expressionRepository
    )
    {
        RuleFor(x => x.ExpressionId)
            .NotEmpty()
            .WithMessage("ExpressionId is required.")
            .MustAsync(async (x, y) => await expressionRepository.GetExpressionForDeletion(x) != null)
            .WithErrorCode("NotFound")
            .WithMessage("This is not a valid expression.")
            .DependentRules(() =>
            {
                RuleFor(x => x.ExpressionId)
                    .MustAsync(async (x, y) => !(await expressionRepository.GetExpressionForDeletion(x))!.IsDeleted)
                    .WithErrorCode("AlreadyDeleted")
                    .WithMessage("This expression has been deleted.");
            });

        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .MustAsync(async (x, y) => await textRepository.GetExpressionSectionForDeletion(x) != null)
            .WithErrorCode("NotFound")
            .WithMessage("This is not a valid expression section.")
            .DependentRules(() =>
            {
                RuleFor(x => x.Id)
                    .MustAsync(async (x, y) => !(await textRepository.GetExpressionSectionForDeletion(x))!.IsDeleted)
                    .WithErrorCode("AlreadyDeleted")
                    .WithMessage("This expression section has been deleted.");
            })
            ;

    }
}