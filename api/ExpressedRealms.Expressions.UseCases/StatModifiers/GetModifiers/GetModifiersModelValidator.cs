using ExpressedRealms.Expressions.Repository.StatModifier;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Expressions.UseCases.StatModifiers.GetModifiers;

[UsedImplicitly]
internal sealed class GetModifiersModelValidator : AbstractValidator<GetModifiersModel>
{
    public GetModifiersModelValidator(IStatModifierRepository statModifierRepository)
    {
        RuleFor(x => x.GroupId)
            .NotEmpty()
            .WithMessage("Group Id is required.")
            .MustAsync(async (x, y) => await statModifierRepository.GroupIdExists(x))
            .WithMessage("Group does not exist.");
    }
}
