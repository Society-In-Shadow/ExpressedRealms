using ExpressedRealms.DB;
using ExpressedRealms.Powers.Repository.Powers.DTOs.PowerCreate;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Powers.Repository.Powers.DTOs.PowerEdit;

public class EditPowerModelValidator : AbstractValidator<EditPowerModel>
{
    public EditPowerModelValidator(ExpressedRealmsDbContext dbContext)
    {
        RuleFor(x => x.Name).MaximumLength(250).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.GameMechanicEffect).NotEmpty();
        RuleFor(x => x.Limitation);
        RuleFor(x => x.Other);
        RuleFor(x => x.Id)
            .MustAsync(
                async (id, cancellationToken) =>
                {
                    return await dbContext.Powers.AnyAsync(x => x.Id == id, cancellationToken);
                }
            )
            .WithErrorCode("NotFound")
            .WithMessage("This is not a valid Power");

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
    }
}
