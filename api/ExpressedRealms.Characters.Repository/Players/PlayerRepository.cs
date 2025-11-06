using ExpressedRealms.DB;
using ExpressedRealms.DB.Helpers;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerSetup;

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

    public async Task EditAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        await context.CommonSaveChanges(entity, cancellationToken);
    }
}
