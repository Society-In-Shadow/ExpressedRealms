using ExpressedRealms.Blessings.Repository.CharacterBlessings;
using ExpressedRealms.Characters.Repository;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Blessings.UseCases.CharacterBlessingMappings.Delete;

[UsedImplicitly]
internal sealed class DeleteBlessingFromCharacterModelValidator
    : AbstractValidator<DeleteBlessingFromCharacterModel>
{
    public DeleteBlessingFromCharacterModelValidator(
        ICharacterRepository characterRepository,
        ICharacterBlessingRepository mappingRepository
    )
    {

        RuleFor(x => x.CharacterId)
            .NotEmpty()
            .WithMessage("Character Id is required.")
            .MustAsync(async (x, y) => await characterRepository.CharacterExistsAsync(x))
            .WithMessage("The Character does not exist.");
        
        RuleFor(x => x.MappingId)
            .NotEmpty()
            .WithMessage("Mapping Id is required.")
            .MustAsync(
                async (x, y) =>
                    await mappingRepository.MappingAlreadyExists(x)
            )
            .WithMessage("The Blessing Mapping does not exist.");

    }
}
