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
}
