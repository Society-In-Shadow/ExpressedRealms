using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Expressions.UseCases.CharacterFactionMappings.LeaveFaction;

[UsedImplicitly]
internal sealed class LeaveFactionModelValidator : AbstractValidator<LeaveFactionModel>
{
    public LeaveFactionModelValidator()
    {
        RuleFor(x => x.CharacterId).NotEmpty().WithMessage("Character Id is required.");
    }
}
