using ExpressedRealms.DB;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Powers.Repository.PowerPaths.DTOs.PowerPathSorting;

public class EditPowerPathSortModelValidator : AbstractValidator<EditPowerPathSortModel>
{
    public EditPowerPathSortModelValidator(ExpressedRealmsDbContext dbContext)
    {
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
            .WithMessage("This is not a valid Expression Id");
        RuleFor(x => x)
            .MustAsync(
                async (dto, cancellationToken) =>
                {
                    var powerPaths = await dbContext
                        .PowerPaths.Where(x => x.ExpressionId == dto.ExpressionId)
                        .ToListAsync(cancellationToken);

                    return dto
                        .Items.Select(x => x.Id)
                        .OrderBy(id => id)
                        .SequenceEqual(powerPaths.Select(x => x.Id).OrderBy(id => id));
                }
            )
            .WithMessage(
                "There has been changes made to the expression that are not reflected in the UI. Please refresh the page and try again."
            );
        RuleFor(x => x)
            .Must(
                (dto, cancellationToken) =>
                {
                    var startingCount = 0;
                    foreach (var item in dto.Items.OrderBy(x => x.SortOrder))
                    {
                        if (item.SortOrder == startingCount)
                            return true;
                        startingCount++;
                    }

                    return false;
                }
            )
            .WithMessage("The sort order has gaps or duplicate values.");
    }
}
