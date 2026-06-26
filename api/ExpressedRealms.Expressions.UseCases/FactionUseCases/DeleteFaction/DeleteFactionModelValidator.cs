using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Expressions.UseCases.FactionUseCases.DeleteFaction;

[UsedImplicitly]
internal sealed class DeleteFactionModelValidator : AbstractValidator<DeleteFactionModel>
{
    public DeleteFactionModelValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    }
}
