using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Knowledges.Repository;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMapping;
using ExpressedRealms.Knowledges.Repository.Knowledges;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.Create;

[UsedImplicitly]
internal sealed class AddModelValidator : AbstractValidator<AddModel>
{
    public AddModelValidator(
        IKnowledgeRepository knowledgeRepository, 
        ICharacterRepository characterRepository,
        ICharacterKnowledgeRepository mappingRepository,
        IKnowledgeLevelRepository knowledgeLevelRepository
    )
    {
        RuleFor(x => x.KnowledgeId)
            .NotEmpty()
            .WithMessage("Knowledge Id is required.")
            .MustAsync(async (x, y) => await knowledgeRepository.IsExistingKnowledge(x))
            .WithMessage("The Knowledge does not exist.");
        
        RuleFor(x => x.CharacterId)
            .NotEmpty()
            .WithMessage("Character Id is required.")
            .MustAsync(async (x, y) => await characterRepository.CharacterExistsAsync(x))
            .WithMessage("The Character does not exist.");
        
        RuleFor(x => x.KnowledgeLevelId)
            .NotEmpty()
            .WithMessage("Knowledge Level Id is required.")
            .MustAsync(async (x, y) => await knowledgeLevelRepository.KnowledgeLevelExists(x))
            .WithMessage("The Knowledge Level does not exist.");
        
        RuleFor(x => x)
            .MustAsync(async (x , y) =>  !await mappingRepository.MappingAlreadyExists(x.KnowledgeId, x.CharacterId))
            .WithMessage("The knowledge already exists for this character.")
            .WithName(nameof(AddModel.KnowledgeId));

        RuleFor(x => x.Notes)
            .MaximumLength(5000).When(x => !string.IsNullOrWhiteSpace(x.Notes))
            .WithMessage("Notes must be less than 5000 characters.");
    }
}