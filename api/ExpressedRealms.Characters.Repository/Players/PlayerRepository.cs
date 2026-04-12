using ExpressedRealms.Characters.Repository.Players.Dtos;
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

    public Task<bool> PlayerExistsAsync(Guid id)
    {
        return context.Players.AnyAsync(x => x.Id == id, cancellationToken);
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

    public async Task<Player> GetPlayerByCharacterId(int characterId)
    {
        return await context.Players.FirstAsync(
            x => x.Characters.Any(y => y.Id == characterId),
            cancellationToken
        );
    }

    public async Task<Player> GetPlayerByIdForEdit(Guid playerId)
    {
        return await context.Players.FirstAsync(x => x.Id == playerId);
    }
    
    public Task<bool> PlayerNumberExists(int playerNumber)
    {
        return context.Players.AnyAsync(x => x.PlayerNumber == playerNumber, cancellationToken);
    }

    public Task<PlayerBasicInfoDto> GetPlayerBasicInfoAsync(Guid id)
    {
        return context.Players.AsNoTracking().Select(x => new PlayerBasicInfoDto()
            {
                PlayerNumber = x.PlayerNumber
            })
            .FirstAsync();
    }

    public async Task EditAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        await context.CommonSaveChanges(entity, cancellationToken);
    }
}
