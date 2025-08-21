using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Powers.Repository.Powers;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Powers.UseCases.CharacterPower.Edit;

[UsedImplicitly]
internal sealed class UpdatePowerForCharacterModelValidator
    : AbstractValidator<UpdatePowerForCharacterModel>
{
    public UpdatePowerForCharacterModelValidator(ICharacterRepository characterRepository, IPowerRepository powerRepository)
    {
        RuleFor(x => x.CharacterId)
            .NotEmpty()
            .WithMessage("Character Id is required.")
            .MustAsync(async (x, y) => await characterRepository.CharacterExistsAsync(x))
            .WithMessage("The Character does not exist.");
        
        RuleFor(x => x.PowerId)
            .NotEmpty()
            .WithMessage("Power Id is required.")
            .MustAsync(async (x, y) => await powerRepository.IsValidPower(x))
            .WithMessage("The Power does not exist.");

        RuleFor(x => x.Notes)
            .MaximumLength(5000)
            .When(x => !string.IsNullOrWhiteSpace(x.Notes))
            .WithMessage("Notes must be less than 5000 characters.");
    }
}
