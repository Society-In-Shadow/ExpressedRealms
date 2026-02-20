using ExpressedRealms.Events.API.Repositories.EventCheckin;
using FluentValidation;

namespace ExpressedRealms.Characters.UseCases.Characters.RetireCharacter;

internal sealed class RetireCharacterModelValidator : AbstractValidator<RetireCharacterModel>
{
    public RetireCharacterModelValidator(IEventCheckinRepository repository)
    {
        RuleFor(x => x.LookupId)
            .NotEmpty()
            .WithMessage("Lookup Id is required.")
            .Length(8)
            .WithMessage("Lookup Id must be 8 characters long.")
            .MustAsync(async (x, y) => await repository.CheckinIdExistsAsync(x))
            .WithErrorCode("NotFound")
            .WithMessage("Lookup Id does not exist.");
    }
}
