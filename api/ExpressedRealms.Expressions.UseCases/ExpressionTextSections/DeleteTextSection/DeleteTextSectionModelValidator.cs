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

        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x)
            .MustAsync(
                async (x, y) =>
                    await textRepository.GetExpressionSectionForDeletion(x.ExpressionId, x.Id)
                    != null
            )
            .WithErrorCode("NotFound")
            .WithName("Id")
            .WithMessage("This is not a valid expression section.")
            .DependentRules(() =>
            {
                RuleFor(x => x)
                    .MustAsync(
                        async (x, y) =>
                            !(
                                await textRepository.GetExpressionSectionForDeletion(
                                    x.ExpressionId,
                                    x.Id
                                )
                            )!.IsDeleted
                    )
                    .WithErrorCode("AlreadyDeleted")
                    .WithName("Id")
                    .WithMessage("This expression section has been deleted.");
            });
    }
}
