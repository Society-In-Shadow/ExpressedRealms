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

    public async Task<bool> HasDuplicateName(string name, int blessingId = 0)
    {
        if (blessingId != 0)
        {
            return await context
                .Blessings.AsNoTracking()
                .AnyAsync(
                    x => x.Name.ToLower() == name.ToLower() && x.Id != blessingId,
                    cancellationToken
                );
        }
        return await context
            .Blessings.AsNoTracking()
            .AnyAsync(x => x.Name.ToLower() == name.ToLower(), cancellationToken);
    }

    public async Task<bool> HasDuplicateLevelName(int blessingId, string name, int levelId = 0)
    {
        if (levelId != 0)
        {
            return await context
                .BlessingLevels.AsNoTracking()
                .AnyAsync(
                    x => x.Level.ToLower() == name.ToLower() && x.Id != levelId && x.BlessingId == blessingId,
                    cancellationToken
                );
        }
        return await context
            .BlessingLevels.AsNoTracking()
            .AnyAsync(x => x.Level.ToLower() == name.ToLower(), cancellationToken);
    }

    public async Task<Blessing> GetBlessingForEditing(int id)
    {
        return await context.Blessings.FirstAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<BlessingLevel> GetBlessingLevelForEditing(int blessingId, int id)
    {
        return await context.BlessingLevels.FirstAsync(
            x => x.BlessingId == blessingId && x.Id == id,
            cancellationToken
        );
    }

    public async Task EditBlessingAsync(Blessing blessing)
    {
        context.Blessings.Update(blessing);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task EditBlessingLevelAsync(BlessingLevel blessing)
    {
        context.BlessingLevels.Update(blessing);
        await context.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> IsExistingBlessingLevel(int blessingId, int id)
    {
        return context.BlessingLevels.AnyAsync(
            x => x.BlessingId == blessingId && x.Id == id,
            cancellationToken
        );
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
