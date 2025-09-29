using ExpressedRealms.Expressions.Repository.ProgressionPaths;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Expressions.UseCases.ProgressionLevels.Delete;

[UsedImplicitly]
internal sealed class DeleteProgressionLevelModelValidator
    : AbstractValidator<DeleteProgressionLevelModel>
{
    public DeleteProgressionLevelModelValidator(IProgressionPathRepository repository)
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
    }
}
