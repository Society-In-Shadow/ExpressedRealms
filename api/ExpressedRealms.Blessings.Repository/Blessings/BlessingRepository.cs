using ExpressedRealms.DB;
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
        return await context.Blessings.AnyAsync(x => x.Name == name);
    }

    public async Task<int> CreateBlessingAsync(Blessing blessing)
    {
        context.Blessings.Add(blessing);
        await context.SaveChangesAsync(cancellationToken);
        return blessing.Id;
    }
}
