using ExpressedRealms.Blessings.Repository.Blessings;
using ExpressedRealms.Blessings.Repository.CharacterBlessings;
using ExpressedRealms.Characters.Repository;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Blessings.UseCases.CharacterBlessingMappings.Create;

[UsedImplicitly]
internal sealed class AddBlessingToCharacterModelValidator
    : AbstractValidator<AddBlessingToCharacterModel>
{
    public AddBlessingToCharacterModelValidator(
        IBlessingRepository blessingRepository,
        ICharacterRepository characterRepository,
        ICharacterBlessingRepository mappingRepository
    )
    {
        RuleFor(x => x.BlessingId)
            .NotEmpty()
            .WithMessage("Blessing Id is required.")
            .MustAsync(async (x, y) => await blessingRepository.IsExistingBlessing(x))
            .WithMessage("The Blessing does not exist.");

        RuleFor(x => x.CharacterId)
            .NotEmpty()
            .WithMessage("Character Id is required.")
            .MustAsync(async (x, y) => await characterRepository.CharacterExistsAsync(x))
            .WithMessage("The Character does not exist.");

        RuleFor(x => x.BlessingLevelId)
            .NotEmpty()
            .WithMessage("Blessing Level Id is required.")
            .MustAsync(async (x, y) => await blessingRepository.BlessingLevelExists(x))
            .WithMessage("The Blessing Level does not exist.");

        RuleFor(x => x)
            .MustAsync(
                async (x, y) =>
                    !await mappingRepository.MappingAlreadyExists(x.BlessingId, x.CharacterId)
            )
            .WithMessage("The Blessing already exists for this character.")
            .WithName(nameof(AddBlessingToCharacterModel.BlessingId));

        RuleFor(x => x.Notes)
            .MaximumLength(5000)
            .When(x => !string.IsNullOrWhiteSpace(x.Notes))
            .WithMessage("Notes must be less than 5000 characters.");
    }
}
