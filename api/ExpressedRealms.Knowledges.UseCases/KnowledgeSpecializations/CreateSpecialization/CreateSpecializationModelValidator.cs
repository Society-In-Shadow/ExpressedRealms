using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using ExpressedRealms.Knowledges.Repository.KnowledgeSpecializations;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Knowledges.UseCases.KnowledgeSpecializations.CreateSpecialization;

[UsedImplicitly]
internal sealed class CreateSpecializationModelValidator : AbstractValidator<CreateSpecializationModel>
{
    public CreateSpecializationModelValidator(
        IKnowledgeSpecializationRepository specializationRepository,
        ICharacterKnowledgeRepository repository
    )
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(250)
            .WithMessage("Name must be between 1 and 250 characters.");
        
        RuleFor(x => x)
            .MustAsync(async (x, y) => !await repository.HasExistingSpecializationForMapping(x.KnowledgeMappingId, x.Name))
            .WithMessage("Knowledge with this name already exists.")
            .WithName(nameof(CreateSpecializationModel.Name));

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(5000)
            .WithMessage("Description must be between 1 and 5000 characters.");
        
        RuleFor(x => x.Notes)
            .MaximumLength(10000)
            .When(x => !string.IsNullOrWhiteSpace(x.Notes))
            .WithMessage("Notes must be less than 10000 characters.");

        RuleFor(x => x.KnowledgeMappingId)
            .NotEmpty()
            .WithMessage("Mapping Id is required.")
            .MustAsync(async (x, y) => await repository.MappingAlreadyExists(x))
            .WithMessage("The Mapping does not exist.");
    }
}
