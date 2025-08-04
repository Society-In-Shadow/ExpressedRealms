using ExpressedRealms.Knowledges.Repository.Knowledges;
using ExpressedRealms.Knowledges.Repository.KnowledgeSpecializations;
using ExpressedRealms.Knowledges.UseCases.Knowledges.DeleteKnowledge;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Knowledges.UseCases.KnowledgeSpecializations.DeleteSpecialization;

[UsedImplicitly]
internal sealed class DeleteSpecializationModelValidator : AbstractValidator<DeleteSpecializationModel>
{
    public DeleteSpecializationModelValidator(IKnowledgeSpecializationRepository repository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .MustAsync(async (x, y) => await repository.SpecializationExists(x))
            .WithMessage("NotFound")
            .WithMessage("This Specialization was not found.");
    }
}
