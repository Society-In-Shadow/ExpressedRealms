using ExpressedRealms.DB;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Powers;
using ExpressedRealms.Repositories.Powers.Powers.DTOs;
using ExpressedRealms.Repositories.Powers.Powers.DTOs.Options;
using ExpressedRealms.Repositories.Powers.Powers.DTOs.PowerCreate;
using ExpressedRealms.Repositories.Powers.Powers.DTOs.PowerEdit;
using ExpressedRealms.Repositories.Shared.CommonFailureTypes;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Repositories.Powers.Powers;

internal sealed class PowerRepository(
    ExpressedRealmsDbContext context,
    CreatePowerModelValidator createPowerModelValidator,
    EditPowerModelValidator editPowerModelValidator,
    CancellationToken cancellationToken
) : IPowerRepository
{
    public async Task<Result<List<PowerInformation>>> GetPowersAsync(int expressionId)
    {
        var items = await context
            .Powers.Where(x => x.ExpressionId == expressionId)
            .Select(x => new PowerInformation
            {
                Id = x.Id,
                Name = x.Name,
                Category = x
                    .CategoryMappings.Select(y => new DetailedInformation(
                        y.Category.Name,
                        y.Category.Description
                    ))
                    .ToList(),
                Description = x.Description,
                GameMechanicEffect = x.GameMechanicEffect,
                Limitation = x.Limitation,
                PowerDuration = new DetailedInformation(
                    x.PowerDuration.Name,
                    x.PowerDuration.Description
                ),
                AreaOfEffect = new DetailedInformation(
                    x.PowerAreaOfEffectType.Name,
                    x.PowerAreaOfEffectType.Description
                ),
                PowerLevel = new DetailedInformation(x.PowerLevel.Name, x.PowerLevel.Description),
                PowerActivationType = new DetailedInformation(
                    x.PowerActivationTimingType.Name,
                    x.PowerActivationTimingType.Description
                ),
                Other = x.OtherFields,
                IsPowerUse = x.IsPowerUse,
            })
            .ToListAsync(cancellationToken);

        return Result.Ok(items);
    }

    public async Task<Result<PowerOptions>> GetPowerOptionsAsync()
    {
        return Result.Ok(
            new PowerOptions()
            {
                AreaOfEffect = await context
                    .PowerAreaOfEffectTypes.AsNoTracking()
                    .Select(x => new DetailedEditInformation(x))
                    .ToListAsync(cancellationToken),
                Category = await context
                    .PowerCategories.AsNoTracking()
                    .Select(x => new DetailedEditInformation(x))
                    .ToListAsync(cancellationToken),
                PowerDuration = await context
                    .PowerDurations.AsNoTracking()
                    .Select(x => new DetailedEditInformation(x))
                    .ToListAsync(cancellationToken),
                PowerLevel = await context
                    .PowerLevels.AsNoTracking()
                    .Select(x => new DetailedEditInformation(x))
                    .ToListAsync(cancellationToken),
                PowerActivationType = await context
                    .PowerActivationTimingTypes.AsNoTracking()
                    .Select(x => new DetailedEditInformation(x))
                    .ToListAsync(cancellationToken),
            }
        );
    }

    public async Task<Result<int>> CreatePower(CreatePowerModel createPowerModel)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            createPowerModelValidator,
            createPowerModel,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var newPower = new Power
        {
            Name = createPowerModel.Name,
            Description = createPowerModel.Description,
            LevelId = createPowerModel.PowerLevel,
            AreaOfEffectTypeId = createPowerModel.AreaOfEffect,
            ActivationTimingTypeId = createPowerModel.PowerActivationType,
            DurationId = createPowerModel.PowerDuration,
            ExpressionId = createPowerModel.ExpressionId,
            IsPowerUse = createPowerModel.IsPowerUse,
            GameMechanicEffect = createPowerModel.GameMechanicEffect,
            Limitation = createPowerModel.Limitation,
            OtherFields = createPowerModel.Other,
        };

        context.Powers.Add(newPower);
        await context.SaveChangesAsync(cancellationToken);

        context.PowerCategoryMappings.AddRange(
            createPowerModel.Category.Select(x => new PowerCategoryMapping()
            {
                PowerId = newPower.Id,
                CategoryId = x,
            })
        );

        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok(newPower.Id);
    }

    public async Task<Result<int>> EditPower(EditPowerModel editPowerModel)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            editPowerModelValidator,
            editPowerModel,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var power = await context.Powers.FirstAsync(
            x => x.Id == editPowerModel.Id,
            cancellationToken
        );

        power.Name = editPowerModel.Name;
        power.Description = editPowerModel.Description;
        power.LevelId = editPowerModel.PowerLevel;
        power.AreaOfEffectTypeId = editPowerModel.AreaOfEffect;
        power.ActivationTimingTypeId = editPowerModel.PowerActivationType;
        power.DurationId = editPowerModel.PowerDuration;
        power.ExpressionId = editPowerModel.ExpressionId;
        power.IsPowerUse = editPowerModel.IsPowerUse;
        power.GameMechanicEffect = editPowerModel.GameMechanicEffect;
        power.Limitation = editPowerModel.Limitation;
        power.OtherFields = editPowerModel.Other;

        await context.SaveChangesAsync(cancellationToken);

        var categoryMappings = await context
            .PowerCategoryMappings.Where(x => x.PowerId == power.Id)
            .ToListAsync(cancellationToken);

        context.Remove(categoryMappings);

        context.PowerCategoryMappings.AddRange(
            editPowerModel.Category.Select(x => new PowerCategoryMapping()
            {
                PowerId = editPowerModel.Id,
                CategoryId = x,
            })
        );

        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    public async Task<Result> DeletePowerAsync(int id)
    {
        var section = await context
            .Powers.IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (section is null)
            return Result.Fail(new NotFoundFailure("Power"));

        if (section.IsDeleted)
            return Result.Fail(new AlreadyDeletedFailure("Power"));

        section.SoftDelete();
        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
