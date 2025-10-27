using ExpressedRealms.Events.API.Repositories.Events;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Events.API.UseCases.Events.SendEventPublishedMessage;

[UsedImplicitly]
internal sealed class SendEventPublishedMessagesModelValidator : AbstractValidator<SendEventPublishedMessagesModel>
{
    public SendEventPublishedMessagesModelValidator(IEventRepository repository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .MustAsync(async (x, y) => await repository.IsExistingEvent(x))
            .WithErrorCode("NotFound")
            .WithMessage("Event does not exist.");
    }
}
