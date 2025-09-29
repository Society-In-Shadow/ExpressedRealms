using ExpressedRealms.Expressions.Repository.ProgressionPaths;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Expressions.UseCases.ProgressionLevels.Edit;

[UsedImplicitly]
internal sealed class EditProgressionLevelModelValidator
    : AbstractValidator<EditProgressionLevelModel>
{
    public EditProgressionLevelModelValidator(IProgressionPathRepository repository)
    {
        RuleFor(x => x.ProgressionLevelId).NotEmpty().WithMessage("Progression Id is required.");

        RuleFor(x => x)
            .MustAsync(
                async (x, y) =>
                    await repository.ProgressionLevelExists(
                        x.ProgressionPathId,
                        x.ProgressionLevelId
                    )
            )
            .WithMessage("Progression Level does not exist.");

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
