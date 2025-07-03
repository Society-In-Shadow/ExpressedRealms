using ExpressedRealms.Powers.Repository.Powers;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Powers.Repository.PowerPrerequisites.GetPrerequisiteUseCase;

[UsedImplicitly]
internal sealed class GetPrerequisiteModelValidator : AbstractValidator<GetPrerequisiteModel>
{
    public GetPrerequisiteModelValidator(IPowerRepository powerRepository)
    {
        RuleFor(x => x.PowerId)
            .NotEmpty()
            .WithMessage("Power Id is required.")
            .MustAsync(async (x, y) => await powerRepository.IsValidPower(x))
            .WithMessage("This is not a valid power id.");
    }
}
