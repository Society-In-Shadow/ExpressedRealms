using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Powers.Repository.CharacterPower;
using ExpressedRealms.Powers.Repository.Powers;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Powers.UseCases.CharacterPower.Create;

[UsedImplicitly]
internal sealed class AddPowerToCharacterModelValidator
    : AbstractValidator<AddPowerToCharacterModel>
{
    public AddPowerToCharacterModelValidator(
        IPowerRepository powerRepository,
        ICharacterRepository characterRepository,
        ICharacterPowerRepository mappingRepository
    )
    {
        RuleFor(x => x.PowerId)
            .NotEmpty()
            .WithMessage("Power Id is required.")
            .MustAsync(async (x, y) => await powerRepository.IsValidPower(x))
            .WithMessage("The Power does not exist.");

        RuleFor(x => x.CharacterId)
            .NotEmpty()
            .WithMessage("Character Id is required.")
            .MustAsync(async (x, y) => await characterRepository.CharacterExistsAsync(x))
            .WithMessage("The Character does not exist.");

        RuleFor(x => x)
            .MustAsync(
                async (x, y) =>
                    !await mappingRepository.MappingExistsAsync(x.PowerId, x.CharacterId)
            )
            .WithMessage("The power already exists for this character.")
            .WithName(nameof(AddPowerToCharacterModel.PowerId));

        RuleFor(x => x)
            .MustAsync(
                async (x, y) =>
                {
                    var availablePowerIds = await mappingRepository.GetSelectablePowersForCharacter(
                        x.CharacterId
                    );
                    return availablePowerIds.Contains(x.PowerId);
                }
            )
            .WithMessage("The character does not have the powers required to use this power.")
            .WithName(nameof(AddPowerToCharacterModel.PowerId));

        RuleFor(x => x.Notes)
            .MaximumLength(5000)
            .When(x => !string.IsNullOrWhiteSpace(x.Notes))
            .WithMessage("Notes must be less than 5000 characters.");
    }
}
