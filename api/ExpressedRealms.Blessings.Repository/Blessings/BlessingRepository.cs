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
        return await context.Blessings.AnyAsync(x => x.Name == name, cancellationToken);
    }

    public async Task<Blessing> GetBlessing(int id)
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
}
