using ExpressedRealms.DB;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Powers.Repository.Powers.DTOs.PowerSorting;

public class EditPowerSortModelValidator : AbstractValidator<EditPowerSortModel>
{
    public EditPowerSortModelValidator(ExpressedRealmsDbContext dbContext)
    {
        RuleFor(x => x.PowerPathId)
            .MustAsync(
                async (powerPathId, cancellationToken) =>
                {
                    return await dbContext.PowerPaths.AnyAsync(
                        x => x.Id == powerPathId,
                        cancellationToken
                    );
                }
            )
            .WithMessage("This is not a valid Power Path Id");
        RuleFor(x => x)
            .MustAsync(
                async (dto, cancellationToken) =>
                {
                    var powers = await dbContext
                        .Powers.Where(x => x.PowerPathId == dto.PowerPathId)
                        .ToListAsync(cancellationToken);

                    return dto
                        .Items.Select(x => x.Id)
                        .OrderBy(id => id)
                        .SequenceEqual(powers.Select(x => x.Id).OrderBy(id => id));
                }
            )
            .WithMessage(
                "There has been changes made to the power path that are not reflected in the UI. Please refresh the page and try again."
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

        RuleFor(x => x.Items)
            .NotEmpty()
            .Must(x => x.Count > 2)
            .WithMessage("You must have at least 2 items to sort.");
    }
}
