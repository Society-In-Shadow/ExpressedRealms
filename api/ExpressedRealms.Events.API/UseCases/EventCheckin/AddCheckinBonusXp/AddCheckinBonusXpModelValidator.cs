using ExpressedRealms.Events.API.Repositories.EventCheckin;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.AddCheckinBonusXp;

[UsedImplicitly]
internal sealed class AddCheckinBonusXpModelValidator : AbstractValidator<AddCheckinBonusXpModel>
{
    public AddCheckinBonusXpModelValidator(IEventCheckinRepository repository)
    {
        RuleFor(x => x.LookupId)
            .NotEmpty()
            .WithMessage("Lookup Id is required.")
            .Length(8)
            .WithMessage("Lookup Id must be 8 characters long.")
            .MustAsync(async (x, y) => await repository.CheckinIdExistsAsync(x))
            .WithErrorCode("NotFound")
            .WithMessage("Lookup Id does not exist.");

        RuleFor(x => x.AssignedXpTypeId)
            .NotEmpty()
            .WithMessage("Assigned Xp Type Id is required.")
            .Must(
                (x) =>
                {
                    List<int> validXpTypes = [2, 4, 5]; // checkin bonus, first time player, brought friend
                    return validXpTypes.Contains(x);
                }
            )
            .WithMessage("The Assigned Xp Type Id is not the right type.");

        RuleFor(x => x.Amount)
            .NotEmpty()
            .WithMessage("Amount is required.")
            .GreaterThan(0)
            .WithMessage("Amount must be greater than 0.")
            .LessThanOrEqualTo(5)
            .WithMessage("Amount must be less than or equal to 5.");
    }
}
