using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.UseCases.Shared;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Events.API.UseCases.Events.Create;

[UsedImplicitly]
internal sealed class CreateEventModelValidator : AbstractValidator<CreateEventModel>
{
    public CreateEventModelValidator(IEventRepository repository)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(250).WithMessage("Name must be between 1 and 250 characters.");
        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start Date is required.");
        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End Date is required.");
        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Location is required.")
            .MaximumLength(1000).WithMessage("Location must be between 1 and 1000 characters.");
        RuleFor(x => x.WebsiteName)
            .NotEmpty().WithMessage("Website Name is required.")
            .MaximumLength(250).WithMessage("Website Name must be between 1 and 250 characters.");
        RuleFor(x => x.WebsiteUrl)
            .NotEmpty().WithMessage("Website Url is required.")
            .MaximumLength(500).WithMessage("Website Url must be between 1 and 500 characters.")
            .MustBeAValidUrl();
        RuleFor(x => x.AdditionalNotes)
            .NotEmpty().WithMessage("Additional Notes is required.")
            .MaximumLength(5000).WithMessage("Additional Notes must be between 1 and 5000 characters.");
        RuleFor(x => x.ConExperience)
            .NotEmpty().WithMessage("Con Experience is required.");
    }
}
