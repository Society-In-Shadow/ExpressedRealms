using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using ExpressedRealms.Knowledges.Repository.KnowledgeSpecializations;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Knowledges.UseCases.KnowledgeSpecializations.CreateSpecialization;

[UsedImplicitly]
internal sealed class CreateSpecializationModelValidator
    : AbstractValidator<CreateSpecializationModel>
{
    public CreateSpecializationModelValidator(
        IKnowledgeSpecializationRepository specializationRepository,
        ICharacterKnowledgeRepository mappingRepository
    )
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(250)
            .WithMessage("Name must be between 1 and 250 characters.");

        RuleFor(x => x)
            .MustAsync(
                async (x, y) =>
                    !await mappingRepository.HasExistingSpecializationForMapping(
                        x.KnowledgeMappingId,
                        x.Name
                    )
            )
            .WithMessage("A specialization with this name already exists for the given knowledge.")
            .WithName(nameof(CreateSpecializationModel.Name));

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(5000)
            .WithMessage("Description must be between 1 and 5000 characters.");

        RuleFor(x => x.Notes)
            .MaximumLength(10000)
            .When(x => !string.IsNullOrWhiteSpace(x.Notes))
            .WithMessage("Notes must be less than 10,000 characters.");

        RuleFor(x => x.KnowledgeMappingId)
            .NotEmpty()
            .WithMessage("Knowledge Mapping Id is required.")
            .MustAsync(async (x, y) => await mappingRepository.MappingAlreadyExists(x))
            .WithMessage("The Knowledge Mapping does not exist.");
    }
}
