using ExpressedRealms.Powers.Repository.Powers;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Powers.Repository.PowerPrerequisites.CreatePrerequisiteUseCase;

[UsedImplicitly]
internal class CreatePrerequisiteModelValidator : AbstractValidator<CreatePrerequisiteModel>
{
    public CreatePrerequisiteModelValidator(IPowerRepository powerRepository)
    {
        RuleFor(x => x.PowerId)
            .NotEmpty()
            .WithMessage("Power Id is required.")
            .MustAsync(async (x, y) => await powerRepository.IsValidPower(x))
            .WithMessage("Invalid Power.")
            .MustAsync(async (x, y) => !await powerRepository.RequirementAlreadyExists(x))
            .WithMessage("A Power Requirement already exists for this power.");

        RuleFor(x => x.RequiredAmount)
            .Must(x => x > 0 || x == -1 || x == -2)
            .WithMessage(
                "Required Amount can only be a value greater then 0, or -1 (All) or -2 (Any)"
            );

        RuleFor(x => x.PrerequisitePowerIds)
            .NotEmpty()
            .WithMessage("Prerequisite Powers are required.")
            .MustAsync(async (x, y) => await powerRepository.AreValidPowers(x))
            .WithMessage("One or more prerequisite powers are invalid.");
    }
}
