using ExpressedRealms.Knowledges.Repository.Knowledges;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Knowledges.UseCases.Knowledges.CreateKnowledge;

[UsedImplicitly]
internal sealed class CreateKnowledgeModelValidator : AbstractValidator<CreateKnowledgeModel>
{
    public CreateKnowledgeModelValidator(IKnowledgeRepository repository)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(150)
            .WithMessage("Name must be between 1 and 150 characters.")
            .MustAsync(async (x, y) => !await repository.HasDuplicateName(x))
            .WithMessage("Knowledge with this name already exists.");

        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");

        RuleFor(x => x.KnowledgeTypeId)
            .NotEmpty()
            .WithMessage("Knowledge Type is required.")
            .MustAsync(async (x, y) => await repository.KnowledgeTypeExists(x))
            .WithMessage("The Knowledge Type does not exist.");
    }
}
