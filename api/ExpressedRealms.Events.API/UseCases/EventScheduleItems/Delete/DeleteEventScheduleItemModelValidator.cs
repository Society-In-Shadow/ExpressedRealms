using ExpressedRealms.Authentication.PermissionCollection;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Events.API.UseCases.EventScheduleItems.Delete;

[UsedImplicitly]
internal sealed class DeleteEventScheduleItemModelValidator
    : AbstractValidator<DeleteEventScheduleItemModel>
{
    public DeleteEventScheduleItemModelValidator(
        IEventRepository repository,
        IUserContext userContext
    )
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
            .MustAsync(
                async (x, y) =>
                {
                    var modifyDefault = userContext.CurrentUserHasPermission(
                        Permissions.EventScheduleItem.ModifyDefaults
                    );

                    if (modifyDefault && x == 1)
                    {
                        return true;
                    }
                    return await repository.IsExistingEvent(x);
                }
            )
            .WithMessage("Event does not exist.");
    }
}
