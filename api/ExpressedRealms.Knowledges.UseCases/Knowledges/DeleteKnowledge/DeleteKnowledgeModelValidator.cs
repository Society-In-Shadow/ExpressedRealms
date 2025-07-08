using ExpressedRealms.Knowledges.Repository.Knowledges;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Knowledges.UseCases.Knowledges.DeleteKnowledge;

[UsedImplicitly]
internal sealed class DeleteKnowledgeModelValidator : AbstractValidator<DeleteKnowledgeModel>
{
    public DeleteKnowledgeModelValidator(IKnowledgeRepository repository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .MustAsync(async (x, y) => await repository.IsExistingKnowledge(x))
            .WithMessage("NotFound")
            .WithMessage("This knowledge was not found.");
    }
}
