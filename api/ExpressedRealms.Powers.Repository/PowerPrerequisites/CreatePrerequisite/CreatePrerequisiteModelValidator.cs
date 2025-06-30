using ExpressedRealms.Powers.Repository.Powers;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Powers.Repository.PowerPrerequisites.CreatePrerequisite;

[UsedImplicitly]
internal class CreatePrerequisiteModelValidator : AbstractValidator<CreatePrerequisiteModel>
{
    public CreatePrerequisiteModelValidator(IPowerRepository powerRepository)
    {
        RuleFor(x => x.PowerId)
            .NotEmpty()
            .MustAsync(async (x, y) => await powerRepository.IsValidPower(x))
            .WithMessage("Invalid Power.");
        
        RuleFor(x => x.PowerId)
            .NotEmpty()
            .MustAsync(async (x, y) => !await powerRepository.RequirementAlreadyExists(x))
            .WithMessage("A Power Requirement already exists for this power.");

        RuleFor(x => x.RequiredAmount)
            .Must(x => x > 0 || x == -1 || x == -2)
            .WithMessage("Required Amount can only be a value greater then 0, or -1 (All) or -2 (Any)");
        
        RuleFor(x => x.PrerequisitePowerIds)
            .NotEmpty()
            .MustAsync(async (x, y) => await powerRepository.AreValidPowers(x))
            .WithMessage("One or more prerequisite powers are invalid.");
    }
}