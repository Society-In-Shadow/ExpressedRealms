using ExpressedRealms.Characters.Repository;
using FluentValidation;

namespace ExpressedRealms.Characters.UseCases.Characters.ToggleExtraMortisUseCase;

internal sealed class ToggleExtraMortisModelValidator : AbstractValidator<ToggleExtraMortisModel>
{
    public ToggleExtraMortisModelValidator(ICharacterRepository characterRepository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .GreaterThan(0)
            .MustAsync(async (x, y) => await characterRepository.FindCharacterAsync(x) is not null)
            .WithMessage("Character does not exist.");
    }
}
