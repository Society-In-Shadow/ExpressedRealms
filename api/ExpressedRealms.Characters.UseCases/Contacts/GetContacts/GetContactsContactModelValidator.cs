using ExpressedRealms.Characters.Repository;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Characters.UseCases.Contacts.GetContacts;

[UsedImplicitly]
internal sealed class GetContactsModelValidator : AbstractValidator<GetContactsModel>
{
    public GetContactsModelValidator(
        ICharacterRepository characterRepository
    )
    {
        RuleFor(x => x.CharacterId)
            .NotEmpty()
            .WithMessage("Character Id is required.")
            .MustAsync(async (x, y) => await characterRepository.FindCharacterAsync(x) is not null)
            .WithMessage("The Character does not exist.");

    }
}
