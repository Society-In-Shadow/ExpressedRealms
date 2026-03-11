using ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerAgeGroupSetup;
using ExpressedRealms.Events.API.Repositories.EventCheckin;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.UpdateAgeInformation;

[UsedImplicitly]
internal sealed class UpdateAgeInformationModelValidator
    : AbstractValidator<UpdateAgeInformationModel>
{
    public UpdateAgeInformationModelValidator(IEventCheckinRepository repository)
    {
        RuleFor(x => x.LookupId)
            .NotEmpty()
            .WithMessage("Lookup Id is required.")
            .Length(8)
            .WithMessage("Lookup Id must be 8 characters long.")
            .MustAsync(async (x, y) => await repository.CheckinIdExistsAsync(x))
            .WithErrorCode("NotFound")
            .WithMessage("Lookup Id does not exist.");

        RuleFor(x => x.AgeGroupId)
            .NotEmpty()
            .WithMessage("Age Group Id is required.")
            .Must(x => PlayerAgeGroupEnum.TryFromValue(x, out _))
            .WithMessage("Age Group Id is not valid.");
    }
}
