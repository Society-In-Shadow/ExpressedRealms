using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.Expressions.Repository.ProgressionPaths;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Expressions.UseCases.ProgressionPaths.Edit;

[UsedImplicitly]
internal sealed class EditProgressionPathModelValidator : AbstractValidator<EditProgressionPathModel>
{
    public EditProgressionPathModelValidator(IExpressionRepository repository, IProgressionPathRepository progressionRepository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Progression Path Id is required.")
            .MustAsync(async (x, y) => await progressionRepository.ProgressionPathExists(x))
            .WithMessage("Progression Path does not exist.");
        
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
