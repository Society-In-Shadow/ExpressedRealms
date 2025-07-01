using ExpressedRealms.Powers.Repository.Powers;
using FluentValidation;

namespace ExpressedRealms.Powers.Repository.PowerPrerequisites.DeletePrerequisiteUseCase;

public class DeletePrerequisiteModelValidator : AbstractValidator<DeletePrerequisiteModel>
{
    public DeletePrerequisiteModelValidator(IPowerRepository powerRepository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .MustAsync(async (x, y) => await powerRepository.IsValidRequirement(x))
            .WithMessage("This is not a valid prerequisite id.");
    }
}
