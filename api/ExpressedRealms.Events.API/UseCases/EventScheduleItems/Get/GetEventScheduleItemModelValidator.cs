using ExpressedRealms.Authentication;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Events.API.UseCases.EventScheduleItems.Get;

[UsedImplicitly]
internal sealed class GetEventScheduleItemModelValidator
    : AbstractValidator<GetEventScheduleItemModel>
{
    public GetEventScheduleItemModelValidator(IEventRepository repository, IUserContext userContext)
    {
        RuleFor(x => x.EventId)
            .NotEmpty()
            .WithMessage("Event Id is required.")
            .MustAsync(async (x, y) => await repository.FindEventAsync(x) is not null)
            .WithMessage("Event does not exist.")
            .MustAsync(
                async (x, y) =>
                {
                    var thisEvent = await repository.FindEventAsync(x);
                    var hasPolicy = await userContext.CurrentUserHasPolicy(Policies.ManageEvents);

                    return hasPolicy || thisEvent!.IsPublished;
                }
            )
            .WithMessage("Event does not exist.");
    }
}
