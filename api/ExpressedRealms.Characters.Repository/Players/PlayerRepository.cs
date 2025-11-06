using ExpressedRealms.DB;
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
}