using ExpressedRealms.Events.API.Repositories.EventCheckin;
using FluentValidation;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.GetBreakOfDawnInfo;

internal sealed class GetBreakOfDawnInfoModelValidator : AbstractValidator<GetBreakOfDawnInfoModel>
{
    public GetBreakOfDawnInfoModelValidator(IEventCheckinRepository repository)
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
