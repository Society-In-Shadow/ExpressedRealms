using ExpressedRealms.Events.API.Repositories.EventCheckin;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.ApproveStageAndSendMessages;

[UsedImplicitly]
internal sealed class ApproveStageAndSendMessageModelValidator
    : AbstractValidator<ApproveStageAndSendMessageModel>
{
    public ApproveStageAndSendMessageModelValidator(IEventCheckinRepository repository)
    {
        RuleFor(x => x.LookupId)
            .NotEmpty()
            .WithMessage("Lookup Id is required if Character Id is empty.")
            .When(x => x.CharacterId is null)
            .Length(8)
            .WithMessage("Lookup Id must be 8 characters long.")
            .When(x => x.CharacterId is null);

        RuleFor(x => x.CharacterId)
            .NotEmpty()
            .WithMessage("Character Id is required if Lookup Id is empty.")
            .When(x => x.LookupId is null);
    }
}
