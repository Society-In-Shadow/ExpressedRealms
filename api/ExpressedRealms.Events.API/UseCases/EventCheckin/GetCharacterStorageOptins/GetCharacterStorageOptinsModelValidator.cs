using ExpressedRealms.Events.API.Repositories.Events;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.GetCharacterStorageOptins;

[UsedImplicitly]
internal sealed class GetCharacterStorageOptinsModelValidator
    : AbstractValidator<GetCharacterStorageOptinsModel>
{
    public GetCharacterStorageOptinsModelValidator(IEventRepository repository)
    {
        RuleFor(x => x.EventId)
            .NotEmpty()
            .WithMessage("Event Id is required.")
            .MustAsync(async (x, y) => await repository.IsExistingEvent(x))
            .WithErrorCode("NotFound")
            .WithMessage("Event Id does not exist.");
    }
}
