using ExpressedRealms.DB;
using ExpressedRealms.DB.Models.Powers.PowerPrerequisitePowerSetup;
using ExpressedRealms.DB.Models.Powers.PowerPrerequisiteSetup;
using ExpressedRealms.Powers.Repository.PowerPrerequisites.DTOs.DeletePrerequisite;
using ExpressedRealms.Powers.Repository.PowerPrerequisites.DTOs.EditPrerequisite;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Powers.Repository.PowerPrerequisites;

public class PowerPrerequisitesRepository(ExpressedRealmsDbContext context, CancellationToken cancellationToken) : IPowerPrerequisitesRepository
{
    public async Task<int> AddPrerequisite(PowerPrerequisite model)
    {
        var entity = context.PowerPrerequisites.Add(model);
        await context.SaveChangesAsync(cancellationToken);
        return entity.Entity.Id;
    }
    
    public async Task AddPrerequisitePowers(List<PowerPrerequisitePower> model)
    {
        context.PowerPrerequisitePowers.AddRange(model);
        await context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<Result> EditPrerequisite(EditPrerequisiteModel model)
    {
        var prerequisite = await context.PowerPrerequisites.FirstAsync(x => x.Id == model.Id, cancellationToken);

        prerequisite.RequiredAmount = model.RequiredAmount;
        
        await context.SaveChangesAsync(cancellationToken);

        var powerPrerequisites = await context.PowerPrerequisitePowers
            .Where(x => x.PrerequisiteId == model.Id)
            .ToListAsync();
        
        context.PowerPrerequisitePowers.RemoveRange(powerPrerequisites);

        if (model.PowerIds.Count == 0)
        {
            await context.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
        
        context.PowerPrerequisitePowers.AddRange(model.PowerIds.Select(x => new PowerPrerequisitePower()
        {
            PrerequisiteId = prerequisite.Id,
            PowerId = x,
        }).ToList());
        
        await context.SaveChangesAsync(cancellationToken);
        
        return Result.Ok();
    }
    
    public async Task<Result> DeletePrerequisite(DeletePrerequisiteModel model)
    {
        var powerPrerequisites = await context.PowerPrerequisitePowers
            .Where(x => x.PrerequisiteId == model.Id)
            .ToListAsync();
        
        context.PowerPrerequisitePowers.RemoveRange(powerPrerequisites);

        var prerequisite = await context.PowerPrerequisites.FirstAsync(x => x.Id == model.Id, cancellationToken);

        context.PowerPrerequisites.Remove(prerequisite);
        await context.SaveChangesAsync(cancellationToken);
        
        return Result.Ok();
    }
}