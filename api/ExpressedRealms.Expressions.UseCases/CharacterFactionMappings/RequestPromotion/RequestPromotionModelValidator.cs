using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Expressions.UseCases.CharacterFactionMappings.RequestPromotion;

[UsedImplicitly]
internal sealed class RequestPromotionModelValidator : AbstractValidator<RequestPromotionModel>
{
    public RequestPromotionModelValidator()
    {
        RuleFor(x => x.CharacterId).NotEmpty().WithMessage("Character Id is required.");

        RuleFor(x => x.FactionLevelId)
            .NotEmpty()
            .WithMessage("Faction Level Id is required.");
        
        RuleFor(x => x.RequestReason)
            .MaximumLength(20_000)
            .When(x => !string.IsNullOrWhiteSpace(x.RequestReason));
    }
}
