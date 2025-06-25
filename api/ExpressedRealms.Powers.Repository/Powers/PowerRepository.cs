using ExpressedRealms.DB;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Powers;
using ExpressedRealms.Powers.Repository.Powers.DTOs.Options;
using ExpressedRealms.Powers.Repository.Powers.DTOs.PowerCreate;
using ExpressedRealms.Powers.Repository.Powers.DTOs.PowerEdit;
using ExpressedRealms.Powers.Repository.Powers.DTOs.PowerList;
using ExpressedRealms.Powers.Repository.Powers.DTOs.PowerSorting;
using ExpressedRealms.Repositories.Shared.CommonFailureTypes;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Powers.Repository.Powers;

internal sealed class PowerRepository(
    ExpressedRealmsDbContext context,
    CreatePowerModelValidator createPowerModelValidator,
    EditPowerModelValidator editPowerModelValidator,
    CancellationToken cancellationToken
) : IPowerRepository
{
    public async Task<Result<List<PowerInformation>>> GetPowersAsync(int powerPathId)
    {
        var items = await context
            .Powers.Where(x => x.PowerPathId == powerPathId)
            .OrderBy(x => x.OrderIndex)
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
                Cost = x.Cost,
            })
            .ToListAsync(cancellationToken);

        return Result.Ok(items);
    }

    public async Task<Result<EditPowerInformation>> GetPowerAsync(int powerId)
    {
        var power = await context
            .Powers.Where(x => x.Id == powerId)
            .Select(x => new EditPowerInformation
            {
                Id = x.Id,
                Name = x.Name,
                CategoryIds = x.CategoryMappings.Select(y => y.CategoryId).ToList(),
                Description = x.Description,
                GameMechanicEffect = x.GameMechanicEffect,
                Limitation = x.Limitation,
                PowerDurationId = x.PowerDuration.Id,
                AreaOfEffectId = x.PowerAreaOfEffectType.Id,
                PowerLevelId = x.PowerLevel.Id,
                PowerActivationTypeId = x.PowerActivationTimingType.Id,
                Other = x.OtherFields,
                IsPowerUse = x.IsPowerUse,
                Cost = x.Cost,
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (power is null)
            return Result.Fail(new NotFoundFailure(nameof(Power)));

        return Result.Ok(power);
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

        var nextPlaceOnList = await context
            .Powers.AsNoTracking()
            .Where(x => x.PowerPathId == createPowerModel.PowerPathId)
            .CountAsync();

        var newPower = new Power
        {
            Name = createPowerModel.Name,
            Description = createPowerModel.Description,
            LevelId = createPowerModel.PowerLevel,
            AreaOfEffectTypeId = createPowerModel.AreaOfEffect,
            ActivationTimingTypeId = createPowerModel.PowerActivationType,
            DurationId = createPowerModel.PowerDuration,
            PowerPathId = createPowerModel.PowerPathId,
            IsPowerUse = createPowerModel.IsPowerUse,
            GameMechanicEffect = createPowerModel.GameMechanicEffect,
            Limitation = createPowerModel.Limitation,
            OtherFields = createPowerModel.Other,
            Cost = createPowerModel.Cost,
            OrderIndex = nextPlaceOnList + 1,
        };

        context.Powers.Add(newPower);
        await context.SaveChangesAsync(cancellationToken);

        if (createPowerModel.Category == null || createPowerModel.Category.Count > 0)
        {
            return Result.Ok(newPower.Id);
        }

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

    public async Task<Result> EditPower(EditPowerModel editPowerModel)
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
        power.IsPowerUse = editPowerModel.IsPowerUse;
        power.GameMechanicEffect = editPowerModel.GameMechanicEffect;
        power.Limitation = editPowerModel.Limitation;
        power.OtherFields = editPowerModel.Other;
        power.Cost = editPowerModel.Cost;

        await context.SaveChangesAsync(cancellationToken);

        var categoryMappings = await context
            .PowerCategoryMappings.Where(x => x.PowerId == power.Id)
            .ToListAsync(cancellationToken);

        context.PowerCategoryMappings.RemoveRange(categoryMappings);

        if (editPowerModel.Category == null || editPowerModel.Category.Count > 0)
        {
            return Result.Ok();
        }

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

    public async Task<Result> UpdatePowerPathSortOrder(EditPowerSortModel dto)
    {
        var sections = await context
            .Powers.Where(x => x.PowerPathId == dto.PowerPathId)
            .ToListAsync();

        foreach (var item in dto.Items)
        {
            var section = sections.First(x => x.Id == item.Id);
            section.OrderIndex = item.SortOrder;
        }

        await context.SaveChangesAsync();
        return Result.Ok();
    }
}
