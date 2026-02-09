using ExpressedRealms.DB;
using ExpressedRealms.DB.Helpers;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerSetup;
using Microsoft.EntityFrameworkCore;
using NanoidDotNet;

namespace ExpressedRealms.Characters.Repository.Players;

internal sealed class PlayerRepository(
    ExpressedRealmsDbContext context,
    CancellationToken cancellationToken
) : IPlayerRepository
{
    public async Task<Player?> FindPlayerAsync(Guid id)
    {
        return await context.Players.FindAsync([id], cancellationToken);
    }

    public async Task<string> GetUniqueLookupId()
    {
        var lookupId = await Nanoid.GenerateAsync(size: 8);

        while (await context.Players.AnyAsync(x => x.LookupId == lookupId))
        {
            lookupId = await Nanoid.GenerateAsync(size: 8);
        }
        
        return lookupId;
    }

    public async Task EditAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        await context.CommonSaveChanges(entity, cancellationToken);
    }
}
