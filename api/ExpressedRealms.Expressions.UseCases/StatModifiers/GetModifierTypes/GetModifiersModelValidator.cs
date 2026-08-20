using ExpressedRealms.Expressions.Repository.StatModifier;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Expressions.UseCases.StatModifiers.GetModifierTypes;

[UsedImplicitly]
internal sealed class GetModifierTypesModelValidator : AbstractValidator<GetModifierTypesModel>
{
    public GetModifierTypesModelValidator(IStatModifierRepository statModifierRepository)
    {
        RuleFor(x => x.Source)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Source is required.")
            .IsInEnum()
            .WithMessage("Source is not recognized as a valid value.");
    }
}
