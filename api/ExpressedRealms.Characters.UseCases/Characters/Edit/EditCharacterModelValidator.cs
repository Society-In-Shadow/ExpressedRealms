using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Expressions.Repository.ProgressionPaths;
using FluentValidation;

namespace ExpressedRealms.Characters.UseCases.Characters.Edit;

internal sealed class EditCharacterModelValidator : AbstractValidator<EditCharacterModel>
{
    public EditCharacterModelValidator(
        IProgressionPathRepository progressionPathRepository,
        ICharacterRepository characterRepository
    )
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .GreaterThan(0)
            .MustAsync(async (x, y) => await characterRepository.FindCharacterAsync(x) is not null)
            .WithMessage("Character does not exist.");

        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);

        RuleFor(x => x.PrimaryProgressionId)
            .MustAsync(
                async (x, y) => await progressionPathRepository.ProgressionPathExists(x!.Value)
            )
            .When(x => x.PrimaryProgressionId is not null)
            .WithMessage("The primary progression does not exist.");

        RuleFor(x => x.SecondaryProgressionId)
            .MustAsync(
                async (x, y) => await progressionPathRepository.ProgressionPathExists(x!.Value)
            )
            .When(x => x.SecondaryProgressionId is not null)
            .WithMessage("The secondary progression does not exist.");

        RuleFor(x => x)
            .MustAsync(
                async (x, y) => await characterRepository.CanUpdatePrimaryCharacterStatus(x.Id)
            )
            .When(x => x.IsPrimaryCharacter)
            .WithName(nameof(EditCharacterModel.IsPrimaryCharacter))
            .WithMessage("A primary character already exists.");
    }
}
