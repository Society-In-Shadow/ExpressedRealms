using ExpressedRealms.Events.API.Repositories.Events;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Events.API.UseCases.EventScheduleItems.Delete;

[UsedImplicitly]
internal sealed class DeleteEventScheduleItemModelValidator
    : AbstractValidator<DeleteEventScheduleItemModel>
{
    public DeleteEventScheduleItemModelValidator(IEventRepository repository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .MustAsync(async (x, y) => await repository.GetEventScheduleItem(x) is not null)
            .WithErrorCode("NotFound")
            .WithMessage("Event Schedule Item does not exist.");

        RuleFor(x => x.EventId)
            .NotEmpty()
            .WithMessage("Event Id is required.")
            .MustAsync(async (x, y) => await repository.IsExistingEvent(x))
            .WithMessage("Event does not exist.");
    }
}
