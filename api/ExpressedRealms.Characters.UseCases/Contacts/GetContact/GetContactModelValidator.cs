using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Contacts;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Characters.UseCases.Contacts.GetContact;

[UsedImplicitly]
internal sealed class GetContactModelValidator : AbstractValidator<GetContactModel>
{
    public GetContactModelValidator(
        ICharacterRepository characterRepository,
        IContactRepository contactRepository
    )
    {
        RuleFor(x => x.CharacterId)
            .NotEmpty()
            .WithMessage("Character Id is required.")
            .MustAsync(async (x, y) => await characterRepository.FindCharacterAsync(x) is not null)
            .WithMessage("The Character does not exist.");

        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Contact Id is required.")
            .MustAsync(async (x, y) => await contactRepository.FindContactAsync(x) is not null)
            .WithMessage("The Contact does not exist.");
    }
}
