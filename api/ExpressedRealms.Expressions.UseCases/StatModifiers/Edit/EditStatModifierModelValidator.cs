using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.Expressions.Repository.StatModifier;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Expressions.UseCases.StatModifiers.Edit;

[UsedImplicitly]
internal sealed class EditStatModifierModelValidator : AbstractValidator<EditStatModifierModel>
{
    public EditStatModifierModelValidator(IStatModifierRepository statModifierRepository, IExpressionRepository expressionRepository)
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Stat Modifier Id is required.");

        RuleFor(x => x.StatModifierGroupId).NotEmpty().WithMessage("Stat Group Id is required.");

        RuleFor(x => x.StatModifierId)
            .NotEmpty()
            .WithMessage("Stat Group Id is required.")
            .MustAsync(async (x, y) => await statModifierRepository.ModifierTypeExists(x));

        RuleFor(x => x)
            .MustAsync(
                async (x, y) =>
                    await statModifierRepository.GroupMappingExists(x.StatModifierGroupId, x.Id)
            )
            .WithMessage("Stat Modifier does not exist.");
        
        RuleFor(x => x.TargetExpressionId)
            .MustAsync(async (x, y) => await expressionRepository.ExpressionExists(x.Value) is null)
            .When(x => x.TargetExpressionId.HasValue)
            .WithMessage("The Expression does not exist.");
    }
}
