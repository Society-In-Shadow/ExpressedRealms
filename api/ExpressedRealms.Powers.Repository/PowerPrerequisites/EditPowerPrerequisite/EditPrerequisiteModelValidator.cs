using ExpressedRealms.Powers.Repository.Powers;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Powers.Repository.PowerPrerequisites.EditPowerPrerequisite;

[UsedImplicitly]
internal class EditPrerequisiteModelValidator : AbstractValidator<EditPrerequisiteModel>
{
    public EditPrerequisiteModelValidator(IPowerRepository powerRepository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .MustAsync(async (x, y) => !await powerRepository.IsValidPower(x));
        
        RuleFor(x => x.RequiredAmount)
            .NotEmpty();

        RuleFor(x => x.PowerIds)
            .NotEmpty()
            .MustAsync(async (x, y) => !await powerRepository.AreValidPowers(x));
    }
}