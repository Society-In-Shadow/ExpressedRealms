using ExpressedRealms.Powers.Repository.CharacterPower;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Powers.UseCases.CharacterPower.Edit;

[UsedImplicitly]
internal sealed class UpdatePowerForCharacterModelValidator
    : AbstractValidator<UpdatePowerForCharacterModel>
{
    public UpdatePowerForCharacterModelValidator(
        ICharacterPowerRepository mappingRepository
    )
    {
        RuleFor(x => x.MappingId)
            .NotEmpty()
            .WithMessage("Power Id is required.")
            .MustAsync(async (x, y) => await mappingRepository.IsValidMapping(x))
            .WithMessage("The Power does not exist.");

        RuleFor(x => x.Notes)
            .MaximumLength(5000)
            .When(x => !string.IsNullOrWhiteSpace(x.Notes))
            .WithMessage("Notes must be less than 5000 characters.");
    }
}
