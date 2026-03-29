using ExpressedRealms.Events.API.Repositories.EventCheckin;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.UpdateCrbEmailNotification;

[UsedImplicitly]
internal sealed class UpdateCrbEmailNotificationModelValidator
    : AbstractValidator<UpdateCrbEmailNotificationModel>
{
    public UpdateCrbEmailNotificationModelValidator(IEventCheckinRepository repository)
    {
        // Intentionally left blank - no validation needed
    }
}
