using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Powers.Repository.Powers;
using FluentValidation;

namespace ExpressedRealms.Powers.UseCases.CharacterPower.GetOptions;

public class GetCharacterPowerOptionsModelValidator : AbstractValidator<GetCharacterPowerOptionsModel>
{
    public GetCharacterPowerOptionsModelValidator(ICharacterRepository characterRepository, IPowerRepository powerRepository)
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
                async (x, y) =>
                    await powerRepository.IsValidPowerForCharacter(x.CharacterId, x.PowerId)
            )
            .WithMessage("The Power is not part of the Expression for the Character.")
            .WithName(nameof(GetCharacterPowerOptionsModel.PowerId));
    }
}
