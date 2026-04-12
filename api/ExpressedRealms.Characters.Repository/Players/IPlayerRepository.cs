using ExpressedRealms.Characters.Repository.Players.Dtos;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerSetup;
using ExpressedRealms.Shared;

namespace ExpressedRealms.Characters.Repository.Players;

public interface IPlayerRepository : IGenericRepository
{
    Task<Player?> FindPlayerAsync(Guid id);
    Task<bool> PlayerExistsAsync(Guid id);
    Task<string> GetUniqueLookupId();
    Task<Player> GetPlayerByCharacterId(int characterId);
    Task<Player> GetPlayerByIdForEdit(Guid playerId);
    Task<bool> PlayerNumberExists(int playerNumber);
    Task<PlayerBasicInfoDto> GetPlayerBasicInfoAsync(Guid id);
}
