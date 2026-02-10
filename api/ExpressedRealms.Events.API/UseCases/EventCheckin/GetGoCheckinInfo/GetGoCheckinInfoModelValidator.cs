using ExpressedRealms.Events.API.Repositories.EventCheckin;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.GetGoCheckinInfo;

[UsedImplicitly]
internal sealed class GetGoCheckinInfoModelValidator : AbstractValidator<GetGoCheckinInfoModel>
{
    public GetGoCheckinInfoModelValidator(
        IEventCheckinRepository repository
    )
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
