using ExpressedRealms.Powers.Repository.Powers;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Powers.Repository.PowerPrerequisites.EditPrerequisiteUseCase;

[UsedImplicitly]
internal class EditPrerequisiteModelValidator : AbstractValidator<EditPrerequisiteModel>
{
    public EditPrerequisiteModelValidator(IPowerRepository powerRepository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .MustAsync(async (x, y) => await powerRepository.IsValidRequirement(x))
            .WithMessage("This is not a valid prerequisite id.");

        RuleFor(x => x.RequiredAmount)
            .Must(x => x > 0 || x == -1 || x == -2)
            .WithMessage(
                "Required Amount can only be a value greater then 0, or -1 (All) or -2 (Any)"
            );

        RuleFor(x => x.PrerequisitePowerIds)
            .NotEmpty()
            .WithMessage("Prerequisite Power Ids are required.")
            .MustAsync(async (x, y) => await powerRepository.AreValidPowers(x))
            .WithMessage("One or more prerequisite powers are invalid.");
    }
}
