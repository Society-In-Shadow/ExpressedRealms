using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.Expressions.Repository.ProgressionPaths;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Expressions.UseCases.ProgressionPaths.Delete;

[UsedImplicitly]
internal sealed class DeleteProgressionPathModelValidator
    : AbstractValidator<DeleteProgressionPathModel>
{
    public DeleteProgressionPathModelValidator(
        IExpressionRepository repository,
        IProgressionPathRepository progressionRepository
    )
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Progression Path Id is required.")
            .MustAsync(async (x, y) => await progressionRepository.ProgressionPathExists(x))
            .WithMessage("Progression Path does not exist.");
    }
}
