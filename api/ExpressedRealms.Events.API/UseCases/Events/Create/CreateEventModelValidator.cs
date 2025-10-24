using ExpressedRealms.Events.API.Repositories.Events;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Events.API.UseCases.Events.Create;

[UsedImplicitly]
internal sealed class CreateEventModelValidator : AbstractValidator<CreateEventModel>
{
    public CreateEventModelValidator(IEventRepository repository)
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty();
        RuleFor(x => x.Location).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.WebsiteName).NotEmpty().MaximumLength(250);
        RuleFor(x => x.WebsiteUrl).NotEmpty().MaximumLength(500);
        RuleFor(x => x.AdditionalNotes).NotEmpty().MaximumLength(5000);
        RuleFor(x => x.ConExperience).NotEmpty();
    }
}
