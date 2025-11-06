using ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerSetup;

namespace ExpressedRealms.Characters.Repository.Players;

public interface IPlayerRepository
{
    Task<Player?> FindPlayerAsync(Guid id);
}