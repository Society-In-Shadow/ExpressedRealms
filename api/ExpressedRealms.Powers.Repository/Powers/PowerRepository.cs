using ExpressedRealms.DB;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Powers;
using ExpressedRealms.Powers.Repository.Powers.DTOs.Options;
using ExpressedRealms.Powers.Repository.Powers.DTOs.PowerEdit;
using ExpressedRealms.Powers.Repository.Powers.DTOs.PowerList;
using ExpressedRealms.Powers.Repository.Powers.DTOs.PowerSorting;
using ExpressedRealms.Repositories.Shared.CommonFailureTypes;
using ExpressedRealms.UseCases.Shared;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Powers.Repository.Powers;

internal sealed class PowerRepository(
    ExpressedRealmsDbContext context,
    EditPowerModelValidator editPowerModelValidator,
    CancellationToken cancellationToken
) : IPowerRepository
{
    public async Task<Result<List<PowerInformation>>> GetPowersAsync(int powerPathId)
    {
        var items = await context
            .PowerPathPowerMappings.Where(x => x.PowerPathId == powerPathId)
            .OrderBy(x => x.OrderIndex)
            .Select(x => new PowerInformation
            {
                Id = x.Power.Id,
                Name = x.Power.Name,
                Category = x
                    .Power.CategoryMappings.Select(y => new DetailedInformation(
                        y.Category.Name,
                        y.Category.Description
                    ))
                    .ToList(),
                Description = x.Power.Description,
                GameMechanicEffect = x.Power.GameMechanicEffect!,
                Limitation = x.Power.Limitation,
                PowerDuration = new DetailedInformation(
                    x.Power.PowerDuration.Name,
                    x.Power.PowerDuration.Description
                ),
                AreaOfEffect = new DetailedInformation(
                    x.Power.PowerAreaOfEffectType.Name,
                    x.Power.PowerAreaOfEffectType.Description
                ),
                PowerLevel = new DetailedInformation(
                    x.Power.PowerLevel.Name,
                    x.Power.PowerLevel.Description
                ),
                PowerActivationType = new DetailedInformation(
                    x.Power.PowerActivationTimingType.Name,
                    x.Power.PowerActivationTimingType.Description
                ),
                Other = x.Power.OtherFields,
                IsPowerUse = x.Power.IsPowerUse,
                Cost = x.Power.Cost,
                Prerequisites =
                    x.Power.Prerequisite != null
                        ? new PrerequisiteDetails()
                        {
                            RequiredAmount = x.Power.Prerequisite.RequiredAmount,
                            Powers = x
                                .Power.Prerequisite.PrerequisitePowers.Select(x => x.Power.Name)
                                .ToList(),
                        }
                        : null,
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
                StatModifierGroup = x.StatModifierGroupId,
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (power is null)
            return Result.Fail(new NotFoundFailure(nameof(Power)));

        return Result.Ok(power);
    }

    public async Task<int> GetPowerExperienceCost(int powerId)
    {
        return await context
            .Powers.Where(x => x.Id == powerId)
            .Select(x => x.PowerLevel.Xp)
            .FirstOrDefaultAsync(cancellationToken);
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

    public async Task<int> CreatePower(Power power)
    {
        context.Powers.Add(power);
        await context.SaveChangesAsync(cancellationToken);
        return power.Id;
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

        if (editPowerModel.Category == null || editPowerModel.Category.Count == 0)
        {
            await context.SaveChangesAsync(cancellationToken);
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
            .PowerPathPowerMappings.Where(x => x.PowerPathId == dto.PowerPathId)
            .ToListAsync();

        foreach (var item in dto.Items)
        {
            var section = sections.First(x => x.PowerId == item.Id);
            section.OrderIndex = item.SortOrder;
        }

        await context.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<bool> IsValidPower(int id)
    {
        var power = await context.Powers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return power is not null;
    }

    public async Task<bool> IsValidPowerForCharacter(int characterId, int powerId)
    {
        var characterExpressionId = await context
            .Characters.AsNoTracking()
            .Where(x => x.Id == characterId)
            .Select(x => x.ExpressionId)
            .FirstOrDefaultAsync(cancellationToken);

        return await context
            .Powers.AsNoTracking()
            .AnyAsync(
                x =>
                    x.Id == powerId
                    && x.PowerPathPowerMapping!.PowerPath.ExpressionId == characterExpressionId,
                cancellationToken
            );
    }

    public async Task<bool> RequirementAlreadyExists(int id)
    {
        var power = await context.PowerPrerequisites.FirstOrDefaultAsync(
            x => x.PowerId == id,
            cancellationToken
        );
        return power is not null;
    }

    public async Task<bool> IsValidRequirement(int id)
    {
        var power = await context.PowerPrerequisites.FirstOrDefaultAsync(
            x => x.Id == id,
            cancellationToken
        );
        return power is not null;
    }

    public async Task<PowerLevel> GetPowerLevelForPower(int id)
    {
        return await context
            .Powers.AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => x.PowerLevel)
            .FirstAsync();
    }

    public async Task<bool> AreValidPowers(List<int> ids)
    {
        var powers = await context
            .Powers.Where(x => ids.Contains(x.Id))
            .ToListAsync(cancellationToken);
        return powers.Count == ids.Count;
    }

    public async Task AddPowerToFactionLevel(int powerId, int targetId)
    {
        await context
            .FactionLevels.Where(x => x.Id == targetId)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.PowerId, powerId));
    }
    
    public async Task<List<PowerInformation>> GetPowers(List<int> powerIds)
    {
        return await context
            .Powers.Where(x => powerIds.Contains(x.Id))
            .Select(PowerInformation.PowerSelector())
            .ToListAsync(cancellationToken);
    }
}
