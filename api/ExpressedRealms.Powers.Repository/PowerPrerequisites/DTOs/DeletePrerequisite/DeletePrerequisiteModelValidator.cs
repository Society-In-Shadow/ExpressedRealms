using ExpressedRealms.Powers.Repository.Powers;
using FluentValidation;

namespace ExpressedRealms.Powers.Repository.PowerPrerequisites.DTOs.DeletePrerequisite;

public class DeletePrerequisiteModelValidator : AbstractValidator<DeletePrerequisiteModel>
{
    public DeletePrerequisiteModelValidator(IPowerRepository powerRepository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .MustAsync(async (x, y) => !await powerRepository.IsValidPower(x));
    }
}