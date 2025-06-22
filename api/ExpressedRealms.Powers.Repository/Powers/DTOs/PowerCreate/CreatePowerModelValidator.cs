using ExpressedRealms.DB;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Powers.Repository.Powers.DTOs.PowerCreate;

public class CreatePowerModelValidator : AbstractValidator<CreatePowerModel>
{
    public CreatePowerModelValidator(ExpressedRealmsDbContext dbContext)
    {
        RuleFor(x => x.Name).MaximumLength(250).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.GameMechanicEffect).NotEmpty();
        RuleFor(x => x.Limitation);
        RuleFor(x => x.Other);
        RuleFor(x => x.PowerDuration)
            .MustAsync(
                async (powerDurationId, cancellationToken) =>
                {
                    return await dbContext.PowerDurations.AnyAsync(
                        x => x.Id == powerDurationId,
                        cancellationToken
                    );
                }
            )
            .WithMessage("This is not a valid Duration Type");

        RuleFor(x => x.PowerLevel)
            .MustAsync(
                async (powerLevelId, cancellationToken) =>
                {
                    return await dbContext.PowerLevels.AnyAsync(
                        x => x.Id == powerLevelId,
                        cancellationToken
                    );
                }
            )
            .WithMessage("This is not a valid Power Level");

        RuleFor(x => x.Category)
            .MustAsync(
                async (categories, cancellationToken) =>
                {
                    return await dbContext.PowerLevels.AnyAsync(
                        x => categories.Contains(x.Id),
                        cancellationToken
                    );
                }
            )
            .When(x => x.Category != null && x.Category.Any())
            .WithMessage("One or more categories are invalid");

        RuleFor(x => x.AreaOfEffect)
            .MustAsync(
                async (areaOfEffectTypeId, cancellationToken) =>
                {
                    return await dbContext.PowerAreaOfEffectTypes.AnyAsync(
                        x => x.Id == areaOfEffectTypeId,
                        cancellationToken
                    );
                }
            )
            .WithMessage("This is not a valid Area of Effect Type");

        RuleFor(x => x.PowerActivationType)
            .MustAsync(
                async (powerActivationTypeId, cancellationToken) =>
                {
                    return await dbContext.PowerActivationTimingTypes.AnyAsync(
                        x => x.Id == powerActivationTypeId,
                        cancellationToken
                    );
                }
            )
            .WithMessage("This is not a valid Power Activation Type");

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
            .WithErrorCode("NotFound")
            .WithMessage("This is not a valid Power Path");
    }
}
