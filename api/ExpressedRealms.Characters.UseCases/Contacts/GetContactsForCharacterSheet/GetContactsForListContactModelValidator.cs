using ExpressedRealms.Characters.Repository;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Characters.UseCases.Contacts.GetContactsForCharacterSheet;

[UsedImplicitly]
internal sealed class GetContactsForCharacterSheetModelValidator : AbstractValidator<GetContactsForCharacterSheetModel>
{
    public GetContactsForCharacterSheetModelValidator(ICharacterRepository characterRepository)
    {
        RuleFor(x => x.CharacterId)
            .NotEmpty()
            .WithMessage("Character Id is required.")
            .MustAsync(async (x, y) => await characterRepository.FindCharacterAsync(x) is not null)
            .WithMessage("The Character does not exist.");
    }
}
