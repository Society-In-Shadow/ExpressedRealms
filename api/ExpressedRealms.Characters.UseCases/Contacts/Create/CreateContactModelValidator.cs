using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Contacts;
using ExpressedRealms.Knowledges.Repository.Knowledges;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Characters.UseCases.Contacts.Create;

[UsedImplicitly]
internal sealed class CreateContactModelValidator : AbstractValidator<CreateContactModel>
{
    public CreateContactModelValidator(
        IKnowledgeRepository knowledgeRepository,
        ICharacterRepository characterRepository,
        IContactRepository contactRepository
    )
    {
        RuleFor(x => x.KnowledgeId)
            .NotEmpty()
            .WithMessage("Knowledge Id is required.")
            .MustAsync(async (x, y) => await knowledgeRepository.IsExistingKnowledge(x))
            .WithMessage("The Knowledge does not exist.");

        RuleFor(x => x.ContactFrequency)
            .InclusiveBetween((byte)1, (byte)3)
            .WithMessage("Contact Frequency must be between 1 and 3 times per month.");
        
        RuleFor(x => x.KnowledgeLevel)
            .InclusiveBetween((byte)4, (byte)6)
            .WithMessage("Knowledge Level must be between level 4 and 6.");

        RuleFor(x => x.CharacterId)
            .NotEmpty()
            .WithMessage("Character Id is required.")
            .MustAsync(async (x, y) => await characterRepository.FindCharacterAsync(x) is not null)
            .WithMessage("The Character does not exist.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(300)
            .WithMessage("Name must be less than 300 characters.");
                
        RuleFor(x => x)
            .MustAsync(async(x, y) => !await contactRepository.HasDuplicateName(x.CharacterId, x.Name.Trim()))
            .WhenAsync(async (x, y) => await characterRepository.FindCharacterAsync(x.CharacterId) is not null)
            .WithMessage("A contact with this name already exists for this character.")
            .WithName(nameof(CreateContactModel.Name));

        RuleFor(x => x.Notes)
            .MaximumLength(5000)
            .When(x => !string.IsNullOrWhiteSpace(x.Notes))
            .WithMessage("Notes must be less than 5000 characters.");
    }
}
