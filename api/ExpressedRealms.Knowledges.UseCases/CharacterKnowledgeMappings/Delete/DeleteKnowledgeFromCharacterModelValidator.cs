using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.Delete;

[UsedImplicitly]
internal sealed class DeleteKnowledgeFromCharacterModelValidator
    : AbstractValidator<DeleteKnowledgeFromCharacterModel>
{
    public DeleteKnowledgeFromCharacterModelValidator(
        ICharacterKnowledgeRepository mappingRepository,
        ICharacterRepository characterRepository
    )
    {
        RuleFor(x => x.MappingId)
            .NotEmpty()
            .WithMessage("Mapping Id is required.")
            .MustAsync(async (x, y) => await mappingRepository.MappingAlreadyExists(x))
            .WithMessage("The Knowledge Mapping does not exist.");

        RuleFor(x => x.CharacterId)
            .NotEmpty()
            .WithMessage("Character Id is required.")
            .MustAsync(async (x, y) => await characterRepository.CharacterExistsAsync(x))
            .WithMessage("NotFound")
            .WithMessage("This Character was not found.");
    }
}
