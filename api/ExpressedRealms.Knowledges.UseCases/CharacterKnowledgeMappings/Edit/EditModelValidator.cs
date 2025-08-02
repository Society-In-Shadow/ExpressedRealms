using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Knowledges.Repository;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using ExpressedRealms.Knowledges.Repository.Knowledges;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.Edit;

[UsedImplicitly]
internal sealed class EditModelValidator : AbstractValidator<EditModel>
{
    public EditModelValidator(
        IKnowledgeRepository knowledgeRepository,
        ICharacterRepository characterRepository,
        ICharacterKnowledgeRepository mappingRepository,
        IKnowledgeLevelRepository knowledgeLevelRepository
    )
    {
        RuleFor(x => x.MappingId)
            .NotEmpty()
            .WithMessage("Mapping Id is required.")
            .MustAsync(async (x, y) => await mappingRepository.MappingAlreadyExists(x))
            .WithMessage("The Knowledge does not exist.");

        RuleFor(x => x.KnowledgeLevelId)
            .NotEmpty()
            .WithMessage("Knowledge Level Id is required.")
            .MustAsync(async (x, y) => await knowledgeLevelRepository.KnowledgeLevelExists(x))
            .WithMessage("The Knowledge Level does not exist.");

        RuleFor(x => x.Notes)
            .MaximumLength(5000)
            .When(x => !string.IsNullOrWhiteSpace(x.Notes))
            .WithMessage("Notes must be less than 5000 characters.");
    }
}
