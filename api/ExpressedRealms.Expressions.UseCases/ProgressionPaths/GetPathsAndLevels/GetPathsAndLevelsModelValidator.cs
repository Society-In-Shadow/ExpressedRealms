using ExpressedRealms.Expressions.Repository.Expressions;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Expressions.UseCases.ProgressionPaths.GetPathsAndLevels;

[UsedImplicitly]
internal sealed class GetPathsAndLevelsModelValidator
    : AbstractValidator<GetPathsAndLevelsModel>
{
    public GetPathsAndLevelsModelValidator(
        IExpressionRepository repository
    )
    {
        RuleFor(x => x.ExpressionId)
            .NotEmpty()
            .WithMessage("Expression Id is required.")
            .MustAsync(async (x, y) => await repository.ExpressionExists(x) is not null)
            .WithMessage("Expression does not exist.");
    }
}
