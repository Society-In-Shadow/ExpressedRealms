using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Knowledges.Repository;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using ExpressedRealms.Knowledges.Repository.Knowledges;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.Delete;

[UsedImplicitly]
internal sealed class DeleteKnowledgeFromCharacterModelValidator
    : AbstractValidator<DeleteKnowledgeFromCharacterModel>
{
    public DeleteKnowledgeFromCharacterModelValidator(
        ICharacterKnowledgeRepository mappingRepository
    )
    {
        RuleFor(x => x.MappingId)
            .NotEmpty()
            .WithMessage("Mapping Id is required.")
            .MustAsync(async (x, y) => await mappingRepository.MappingAlreadyExists(x))
            .WithMessage("The Knowledge Mapping does not exist.");
    }
}
