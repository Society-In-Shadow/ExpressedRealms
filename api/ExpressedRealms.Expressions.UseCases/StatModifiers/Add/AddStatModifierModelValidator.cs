using ExpressedRealms.Expressions.Repository.StatModifier;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Expressions.UseCases.StatModifiers.Add;

[UsedImplicitly]
internal sealed class AddStatModifierModelValidator : AbstractValidator<AddStatModifierModel>
{
    public AddStatModifierModelValidator(IStatModifierRepository statModifierRepository)
    {
        RuleFor(x => x.SourceTable).NotEmpty().WithMessage("Source Table is required.").IsInEnum();

        RuleFor(x => x)
            .MustAsync(
                async (x, y) =>
                {
                    switch (x.SourceTable)
                    {
                        case SourceTableEnum.ProgressionLevels:
                            return await statModifierRepository.ProgressionPathExists(x.SourceId);
                        case SourceTableEnum.Blessings:
                            return await statModifierRepository.BlessingLevelExists(x.SourceId);
                        case SourceTableEnum.Powers:
                            return await statModifierRepository.PowerExists(x.SourceId);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            )
            .WithMessage("Source Id does not exist in the Corresponding Source Table.");

        RuleFor(x => x.StatModifierGroupId)
            .MustAsync(async (x, y) => await statModifierRepository.GroupIdExists(x!.Value))
            .When(x => x.StatModifierGroupId.HasValue)
            .WithMessage("The Group does not exist.");

        RuleFor(x => x.StatModifierId)
            .NotEmpty()
            .WithMessage("Stat Modifier Id is required.")
            .MustAsync(async (x, y) => await statModifierRepository.ModifierTypeExists(x));
    }
}
