using ExpressedRealms.Knowledges.Repository.Knowledges;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Knowledges.UseCases.Knowledges.EditKnowledge;

[UsedImplicitly]
internal sealed class EditKnowledgeModelValidator : AbstractValidator<EditKnowledgeModel>
{
    public EditKnowledgeModelValidator(IKnowledgeRepository repository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .MustAsync(async (x, y) => await repository.IsExistingKnowledge(x))
            .WithMessage("NotFound")
            .WithMessage("This knowledge was not found.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(150)
            .WithMessage("Name must be between 1 and 150 characters.");

        RuleFor(x => x)
            .MustAsync(async (x, y) => !await repository.HasDuplicateName(x.Name, x.Id))
            .WithName("Name")
            .WithMessage("Knowledge with this name already exists.");

        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");

        RuleFor(x => x.KnowledgeTypeId)
            .NotEmpty()
            .WithMessage("Knowledge Type is required.")
            .MustAsync(async (x, y) => await repository.KnowledgeTypeExists(x))
            .WithMessage("The Knowledge Type does not exist.");
    }
}
