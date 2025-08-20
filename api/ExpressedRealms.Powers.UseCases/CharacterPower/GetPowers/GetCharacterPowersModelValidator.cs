using ExpressedRealms.Characters.Repository;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Powers.UseCases.CharacterPower.GetPowers;

[UsedImplicitly]
internal sealed class GetCharacterPowersModelValidator : AbstractValidator<GetCharacterPowersModel>
{
    public GetCharacterPowersModelValidator(ICharacterRepository characterRepository)
    {
        RuleFor(x => x.CharacterId)
            .NotEmpty()
            .WithMessage("Character Id is required.")
            .MustAsync(async (x, y) => await characterRepository.CharacterExistsAsync(x))
            .WithMessage("The Character does not exist.");
    }
}
