using ExpressedRealms.Expressions.Repository.StatModifier;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Expressions.UseCases.StatModifiers.Delete;

[UsedImplicitly]
internal sealed class DeleteStatModifierModelValidator : AbstractValidator<DeleteStatModifierModel>
{
    public DeleteStatModifierModelValidator(IStatModifierRepository statModifierRepository)
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Stat Modifier Id is required.");

        RuleFor(x => x.StatModifierGroupId).NotEmpty().WithMessage("Stat Group Id is required.");

        RuleFor(x => x)
            .MustAsync(
                async (x, y) =>
                    await statModifierRepository.GroupMappingExists(x.StatModifierGroupId, x.Id)
            )
            .WithMessage("Stat Modifier does not exist.");
    }
}
