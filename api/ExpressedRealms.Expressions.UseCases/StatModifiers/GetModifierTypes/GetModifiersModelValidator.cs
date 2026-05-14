using ExpressedRealms.Expressions.Repository.StatModifier;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Expressions.UseCases.StatModifiers.GetModifierTypes;

[UsedImplicitly]
internal sealed class GetModifierTypesModelValidator : AbstractValidator<GetModifierTypesModel>
{
    public GetModifierTypesModelValidator(IStatModifierRepository statModifierRepository)
    {
        RuleFor(x => x.Source).IsInEnum();
    }
}
