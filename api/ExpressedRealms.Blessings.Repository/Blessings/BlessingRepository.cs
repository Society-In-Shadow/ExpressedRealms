using ExpressedRealms.DB;
using ExpressedRealms.DB.Models.Blessings.BlessingLevelSetup;
using ExpressedRealms.DB.Models.Blessings.BlessingSetup;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Blessings.Repository.Blessings;

internal sealed class BlessingRepository(
    ExpressedRealmsDbContext context,
    CancellationToken cancellationToken
) : IBlessingRepository
{
    public async Task<List<Blessing>> GetAllBlessingsAndBlessingLevels()
    {
        return await context
            .Blessings.AsNoTracking()
            .Include(x => x.BlessingLevels)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasDuplicateName(string name)
    {
        return await context.Blessings.AnyAsync(x => x.Name == name, cancellationToken);
    }

    public async Task<bool> HasDuplicateLevelName(int blessingId, string name)
    {
        return await context.BlessingLevels.AnyAsync(
            x => x.BlessingId == blessingId && x.Level == name,
            cancellationToken
        );
    }

    public async Task<Blessing> GetBlessingForEditing(int id)
    {
        return await context.Blessings.FirstAsync(x => x.Id == id, cancellationToken);
    }

    public async Task EditBlessingAsync(Blessing blessing)
    {
        context.Blessings.Update(blessing);
        await context.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> IsExistingBlessing(int id)
    {
        return context.Blessings.AnyAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<int> CreateBlessingAsync(Blessing blessing)
    {
        context.Blessings.Add(blessing);
        await context.SaveChangesAsync(cancellationToken);
        return blessing.Id;
    }

    public async Task<int> CreateBlessingLevelAsync(BlessingLevel blessingLevel)
    {
        context.BlessingLevels.Add(blessingLevel);
        await context.SaveChangesAsync(cancellationToken);
        return blessingLevel.Id;
    }
}
