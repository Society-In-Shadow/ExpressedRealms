using ExpressedRealms.DB;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Powers.Repository.PowerPaths.DTOs.PowerPathEdit;

public class EditPowerPathModelValidator : AbstractValidator<EditPowerPathModel>
{
    public EditPowerPathModelValidator(ExpressedRealmsDbContext dbContext)
    {
        RuleFor(x => x.Name).MaximumLength(250).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.Id)
            .MustAsync(
                async (id, cancellationToken) =>
                {
                    return await dbContext.PowerPaths.AnyAsync(x => x.Id == id, cancellationToken);
                }
            )
            .WithErrorCode("NotFound")
            .WithMessage("This is not a valid Power Path");
    }
}
