using ExpressedRealms.Events.API.Repositories.Events;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Events.API.UseCases.EventScheduleItems.Create;

[UsedImplicitly]
internal sealed class CreateEventScheduleItemModelValidator : AbstractValidator<CreateEventScheduleItemModel>
{
    public CreateEventScheduleItemModelValidator(IEventRepository repository)
    {
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(250)
            .WithMessage("Description must be between 1 and 250 characters.");
        RuleFor(x => x.StartTime).NotEmpty().WithMessage("Start Date is required.");
        RuleFor(x => x.EndTime).NotEmpty().WithMessage("End Date is required.");
        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is required.");
        RuleFor(x => x)
            .MustAsync(async (x, y) =>
            {
                var parentEvent = await repository.GetEventAsync(x.EventId);
                return parentEvent.StartDate <= x.Date && x.Date <= parentEvent.EndDate;
            })
            .WithMessage("Date must be within the event dates.");
        RuleFor(x => x.EventId)
            .NotEmpty()
            .WithMessage("Event Id is required.")
            .MustAsync(async (x, y) => await repository.IsExistingEvent(x))
            .WithErrorCode("NotFound")
            .WithMessage("Event does not exist.");
    }
}
