using ExpressedRealms.DB;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Repositories.Powers.PowerPaths.DTOs.PowerPathCreate;

public class CreatePowerPathModelValidator : AbstractValidator<CreatePowerPathModel>
{
    public CreatePowerPathModelValidator(ExpressedRealmsDbContext dbContext)
    {
        RuleFor(x => x.Name).MaximumLength(250).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.ExpressionId)
            .MustAsync(
                async (expressionId, cancellationToken) =>
                {
                    return await dbContext.Expressions.AnyAsync(
                        x => x.Id == expressionId,
                        cancellationToken
                    );
                }
            )
            .WithErrorCode("NotFound")
            .WithMessage("This is not a valid Expression Id");
    }
}
