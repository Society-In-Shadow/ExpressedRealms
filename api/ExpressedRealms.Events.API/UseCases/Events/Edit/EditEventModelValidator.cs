using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.UseCases.Shared;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Events.API.UseCases.Events.Edit;

[UsedImplicitly]
internal sealed class EditEventModelValidator : AbstractValidator<EditEventModel>
{
    public EditEventModelValidator(IEventRepository repository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .MustAsync(async (x, y) => await repository.IsExistingEvent(x))
            .WithErrorCode("NotFound")
            .WithMessage("Event does not exist.");
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(250)
            .WithMessage("Name must be between 1 and 250 characters.");
        RuleFor(x => x.StartDate).NotEmpty().WithMessage("Start Date is required.");
        RuleFor(x => x.EndDate).NotEmpty().WithMessage("End Date is required.");
        RuleFor(x => x.Location)
            .NotEmpty()
            .WithMessage("Location is required.")
            .MaximumLength(1000)
            .WithMessage("Location must be between 1 and 1000 characters.");
        RuleFor(x => x.WebsiteName)
            .NotEmpty()
            .WithMessage("Website Name is required.")
            .MaximumLength(250)
            .WithMessage("Website Name must be between 1 and 250 characters.");
        RuleFor(x => x.WebsiteUrl)
            .NotEmpty()
            .WithMessage("Website Url is required.")
            .MaximumLength(500)
            .WithMessage("Website Url must be between 1 and 500 characters.")
            .MustBeAValidUrl();
        RuleFor(x => x.AdditionalNotes)
            .MaximumLength(5000)
            .WithMessage("Additional Notes must be between 1 and 5000 characters.");
        RuleFor(x => x.TimeZoneId)
            .NotEmpty()
            .WithMessage("Time Zone Id is required.")
            .MaximumLength(250)
            .WithMessage("Time Zone Id must be between 1 and 250 characters.")
            .MustBeAValidTimeZone();
        RuleFor(x => x.ConExperience).NotEmpty().WithMessage("Con Experience is required.");
    }
}
