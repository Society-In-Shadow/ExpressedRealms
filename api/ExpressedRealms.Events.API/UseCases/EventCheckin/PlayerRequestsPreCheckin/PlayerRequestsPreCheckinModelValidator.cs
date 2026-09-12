using ExpressedRealms.Events.API.Repositories.EventCheckin;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.PlayerRequestsPreCheckin;

[UsedImplicitly]
internal sealed class PlayerRequestsPreCheckinModelValidator
    : AbstractValidator<PlayerRequestsPreCheckinModel>
{
    public PlayerRequestsPreCheckinModelValidator(IEventCheckinRepository repository)
    {
        RuleFor(x => x.CharacterId)
            .NotEmpty()
            .WithMessage("Character Id is required.");
    }
}
