using ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerSetup;
using ExpressedRealms.Shared;

namespace ExpressedRealms.Characters.Repository.Players;

public interface IPlayerRepository : IGenericRepository
{
    Task<Player?> FindPlayerAsync(Guid id);
    Task<string> GetUniqueLookupId();
}
