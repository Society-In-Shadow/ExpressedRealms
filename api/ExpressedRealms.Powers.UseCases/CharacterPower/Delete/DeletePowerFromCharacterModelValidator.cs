using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Powers.Repository.CharacterPower;
using ExpressedRealms.Powers.Repository.Powers;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Powers.UseCases.CharacterPower.Delete;

[UsedImplicitly]
internal sealed class DeletePowerFromCharacterModelValidator
    : AbstractValidator<DeletePowerFromCharacterModel>
{
    public DeletePowerFromCharacterModelValidator(
        ICharacterRepository characterRepository,
        IPowerRepository powerRepository,
        ICharacterPowerRepository mappingRepository
    )
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

        RuleFor(x => x)
            .MustAsync(
                async (x, y) => await mappingRepository.MappingExistsAsync(x.PowerId, x.CharacterId)
            )
            .WithMessage("The Mapping does not exist.")
            .WithName(nameof(DeletePowerFromCharacterModel.PowerId));

        RuleFor(x => x)
            .MustAsync(
                async (x, y) =>
                    !await mappingRepository.IsPowerPartOfPrerequisite(x.CharacterId, x.PowerId)
            )
            .WithMessage("The Power is Part of a Prerequisite.")
            .WithName(nameof(DeletePowerFromCharacterModel.PowerId));
    }
}
