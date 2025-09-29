using ExpressedRealms.Expressions.Repository.ProgressionPaths;
using FluentValidation;

namespace ExpressedRealms.Characters.Repository.DTOs;

internal sealed class EditCharacterDtoValidator : AbstractValidator<EditCharacterDto>
{
    public EditCharacterDtoValidator(IProgressionPathRepository progressionPathRepository)
    {
        RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.PrimaryProgressionId)
            .MustAsync(
                async (x, y) => await progressionPathRepository.ProgressionPathExists(x!.Value)
            )
            .When(x => x.PrimaryProgressionId is not null);
        RuleFor(x => x.SecondaryProgressionId)
            .MustAsync(
                async (x, y) => await progressionPathRepository.ProgressionPathExists(x!.Value)
            )
            .When(x => x.SecondaryProgressionId is not null);
    }
}
