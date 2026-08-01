using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Expressions.UseCases.CharacterFactionMappings.ApprovePromotion;

[UsedImplicitly]
internal sealed class ApprovePromotionModelValidator : AbstractValidator<ApprovePromotionModel>
{
    public ApprovePromotionModelValidator()
    {
        RuleFor(x => x.CharacterId).NotEmpty().WithMessage("Character Id is required.");

        RuleFor(x => x.FactionLevelId).NotEmpty().WithMessage("Faction Level Id is required.");

        RuleFor(x => x.ApprovalReason)
            .MinimumLength(20)
            .MaximumLength(20_000)
            .NotEmpty()
            .WithMessage("Approval Reason is required.");
    }
}
