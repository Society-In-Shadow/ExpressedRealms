using ExpressedRealms.Expressions.Repository.Expressions;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Expressions.UseCases.ProgressionPaths.Add;

[UsedImplicitly]
internal sealed class AddProgressionPathModelValidator : AbstractValidator<AddProgressionPathModel>
{
    public AddProgressionPathModelValidator(IExpressionRepository repository)
    {
        RuleFor(x => x.ExpressionId)
            .NotEmpty()
            .WithMessage("Expression Id is required.")
            .MustAsync(async (x, y) => await repository.ExpressionExists(x) is not null)
            .WithMessage("Expression does not exist.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(250)
            .WithMessage("Name must be between 1 and 250 characters.");
        
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(5000)
            .WithMessage("Name must be between 1 and 5000 characters.");
    }
}