using ExpressedRealms.DB;
using ExpressedRealms.Repositories.Powers.Powers.DTOs;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Repositories.Powers.Powers;

internal sealed class PowerRepository(ExpressedRealmsDbContext context) : IPowerRepository
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
            }).ToListAsync();
        
        return Result.Ok(items);
    }
}