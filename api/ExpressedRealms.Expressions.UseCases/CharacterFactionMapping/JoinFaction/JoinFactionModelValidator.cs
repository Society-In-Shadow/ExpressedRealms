using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Expressions.UseCases.CharacterFactionMapping.JoinFaction;

[UsedImplicitly]
internal sealed class JoinFactionModelValidator : AbstractValidator<JoinFactionModel>
{
    public JoinFactionModelValidator()
    {
        RuleFor(x => x.CharacterId).NotEmpty().WithMessage("Character Id is required.");

        RuleFor(x => x.FactionId).NotEmpty().WithMessage("Faction Id is required.");
    }
}
