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
            .MaximumLength(150)
            .MustAsync(async (x, y) => !await repository.HasDuplicateName(x))
            .WithMessage("Knowledge with this name already exists.");
        
        RuleFor(x => x.Description)
            .NotEmpty();
            
        RuleFor(x => x.KnowledgeTypeId)
            .NotEmpty()
            .MustAsync(async (x, y) => await repository.KnowledgeTypeExists(x))
            .WithMessage("The Knowledge Type does not exist.");
    }
}