using ExpressedRealms.DB;
using ExpressedRealms.DB.Models.Powers.PowerPrerequisitePowerSetup;
using ExpressedRealms.DB.Models.Powers.PowerPrerequisiteSetup;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Powers.Repository.PowerPrerequisites;

public class PowerPrerequisitesRepository(
    ExpressedRealmsDbContext context,
    CancellationToken cancellationToken
) : IPowerPrerequisitesRepository
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

    public async Task<PowerPrerequisite> GetPrerequisiteForEditingAsync(int id)
    {
        return await context.PowerPrerequisites.FirstAsync(x => x.Id == id, cancellationToken);
    }

    public async Task UpdatePrerequisite(PowerPrerequisite model)
    {
        context.PowerPrerequisites.Update(model);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemovePrerequisitePowers(int prerequisiteId)
    {
        var powerPrerequisites = await context
            .PowerPrerequisitePowers.Where(x => x.PrerequisiteId == prerequisiteId)
            .ToListAsync();

        context.PowerPrerequisitePowers.RemoveRange(powerPrerequisites);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> DeletePrerequisite(int prerequisiteId)
    {
        var prerequisite = await context.PowerPrerequisites.FirstAsync(
            x => x.Id == prerequisiteId,
            cancellationToken
        );

        context.PowerPrerequisites.Remove(prerequisite);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
