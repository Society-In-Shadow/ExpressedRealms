using ExpressedRealms.Expressions.Repository.ProgressionPaths;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Expressions.UseCases.ProgressionLevels.Add;

[UsedImplicitly]
internal sealed class AddProgressionLevelModelValidator
    : AbstractValidator<AddProgressionLevelModel>
{
    public AddProgressionLevelModelValidator(IProgressionPathRepository repository)
    {
        RuleFor(x => x.ProgressionId)
            .NotEmpty()
            .WithMessage("Progression Id is required.")
            .MustAsync(async (x, y) => await repository.ProgressionPathExists(x))
            .WithMessage("Progression does not exist.");

        RuleFor(x => x.XlLevel)
            .GreaterThan(0)
            .WithMessage("XL Level needs to be a positive number");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(5000)
            .WithMessage("Name must be between 1 and 5000 characters.");
    }
}
