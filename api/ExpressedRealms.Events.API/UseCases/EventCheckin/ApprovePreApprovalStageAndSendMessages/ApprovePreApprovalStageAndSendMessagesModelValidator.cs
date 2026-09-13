using ExpressedRealms.Events.API.Repositories.EventCheckin;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.ApprovePreApprovalStageAndSendMessages;

[UsedImplicitly]
internal sealed class ApprovePreApprovalStageAndSendMessageModelValidator
    : AbstractValidator<ApprovePreApprovalStageAndSendMessageModel>
{
    public ApprovePreApprovalStageAndSendMessageModelValidator(IEventCheckinRepository repository)
    {
        RuleFor(x => x.CharacterId)
            .NotEmpty()
            .WithMessage("Character Id is required.");
    }
}
