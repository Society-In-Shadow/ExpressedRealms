using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Knowledges.Repository.KnowledgeSpecializations;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Knowledges.UseCases.KnowledgeSpecializations.DeleteSpecialization;

[UsedImplicitly]
internal sealed class DeleteSpecializationModelValidator
    : AbstractValidator<DeleteSpecializationModel>
{
    public DeleteSpecializationModelValidator(IKnowledgeSpecializationRepository repository, ICharacterRepository characterRepository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .MustAsync(async (x, y) => await repository.SpecializationExists(x))
            .WithMessage("NotFound")
            .WithMessage("This Specialization was not found.");
        
        RuleFor(x => x.CharacterId)
            .NotEmpty()
            .WithMessage("Character Id is required.")
            .MustAsync(async (x, y) => await characterRepository.CharacterExistsAsync(x))
            .WithMessage("NotFound")
            .WithMessage("This Character was not found.");
    }
}
