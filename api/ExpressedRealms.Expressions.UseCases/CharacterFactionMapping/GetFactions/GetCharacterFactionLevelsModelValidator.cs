using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Expressions.UseCases.CharacterFactionMapping.GetFactions;

[UsedImplicitly]
internal sealed class GetCharacterFactionLevelsModelValidator
    : AbstractValidator<GetCharacterFactionLevelsModel>
{
    public GetCharacterFactionLevelsModelValidator()
    {
        RuleFor(x => x.CharacterId).NotEmpty().WithMessage("Character Id is required.");
    }
}
