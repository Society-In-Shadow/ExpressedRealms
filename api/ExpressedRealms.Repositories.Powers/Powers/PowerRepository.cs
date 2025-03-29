using ExpressedRealms.DB;
using ExpressedRealms.DB.Models.Powers;
using ExpressedRealms.Repositories.Powers.Powers.DTOs;
using ExpressedRealms.Repositories.Powers.Powers.DTOs.PowerCreate;
using ExpressedRealms.Repositories.Shared.CommonFailureTypes;
using FluentResults;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Repositories.Powers.Powers;

internal sealed class PowerRepository(
    ExpressedRealmsDbContext context,
    CreatePowerModelValidator createPowerModelValidator,
    CancellationToken cancellationToken
) : IPowerRepository
{
    public async Task<Result<List<PowerInformation>>> GetPowersAsync(int expressionId)
    {
        var items = await context.Powers
            .Where(x => x.ExpressionId == expressionId)
            .Select(x => new PowerInformation
            {
                Id = x.Id,
                Name = x.Name,
                Category = x.CategoryMappings.Select(y => new DetailedInformation(y.Category.Name, y.Category.Description)).ToList(),
                Description = x.Description,
                GameMechanicEffect = x.GameMechanicEffect,
                Limitation = x.Limitation,
                PowerDuration = new DetailedInformation(x.PowerDuration.Name, x.PowerDuration.Description),
                AreaOfEffect = new DetailedInformation(x.PowerAreaOfEffectType.Name, x.PowerAreaOfEffectType.Description),
                PowerLevel = new DetailedInformation(x.PowerLevel.Name, x.PowerLevel.Description),
                PowerActivationType = new DetailedInformation(x.PowerActivationTimingType.Name, x.PowerActivationTimingType.Description),
                Other = x.OtherFields
            }).ToListAsync(cancellationToken);
        
        return Result.Ok(items);
    }

    public async Task<Result<PowerOptions>> GetPowerOptionsAsync()
    {
        return Result.Ok(new PowerOptions()
        {
            AreaOfEffect = await context.PowerAreaOfEffectTypes.AsNoTracking().Select(x => new DetailedEditInformation(x)).ToListAsync(cancellationToken),
            Category = await context.PowerCategories.AsNoTracking().Select(x => new DetailedEditInformation(x)).ToListAsync(cancellationToken),
            PowerDuration = await context.PowerDurations.AsNoTracking().Select(x => new DetailedEditInformation(x)).ToListAsync(cancellationToken),
            PowerLevel = await context.PowerLevels.AsNoTracking().Select(x => new DetailedEditInformation(x)).ToListAsync(cancellationToken),
            PowerActivationType = await context.PowerActivationTimingTypes.AsNoTracking().Select(x => new DetailedEditInformation(x)).ToListAsync(cancellationToken),
        });
    }
    
    public async Task<Result<int>> CreatePower(CreatePowerModel createPowerModel)
    {
        var result = await createPowerModelValidator.ValidateAsync(
            createPowerModel,
            cancellationToken
        );
        
        if (!result.IsValid)
            return Result.Fail(new FluentValidationFailure(result.ToDictionary()));

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
            OtherFields = createPowerModel.Other
        };

        context.Powers.Add(newPower);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok(newPower.Id);

    }
}