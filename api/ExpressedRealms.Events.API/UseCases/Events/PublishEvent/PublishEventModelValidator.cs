using ExpressedRealms.Events.API.Repositories.Events;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Events.API.UseCases.Events.PublishEvent;

[UsedImplicitly]
internal sealed class PublishEventModelValidator : AbstractValidator<PublishEventModel>
{
    public PublishEventModelValidator(IEventRepository repository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .MustAsync(async (x, y) => await repository.FindEventAsync(x) is not null)
            .WithErrorCode("NotFound")
            .WithMessage("Event does not exist.");
    }
}
