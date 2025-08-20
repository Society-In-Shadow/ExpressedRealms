using ExpressedRealms.Powers.Repository.CharacterPower;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Powers.UseCases.CharacterPower.Delete;

[UsedImplicitly]
internal sealed class DeletePowerFromCharacterModelValidator
    : AbstractValidator<DeletePowerFromCharacterModel>
{
    public DeletePowerFromCharacterModelValidator(ICharacterPowerRepository mappingRepository)
    {
        RuleFor(x => x.MappingId)
            .NotEmpty()
            .WithMessage("Mapping Id is required.")
            .MustAsync(async (x, y) => await mappingRepository.IsValidMapping(x))
            .WithMessage("The Mapping does not exist.");
    }
}
