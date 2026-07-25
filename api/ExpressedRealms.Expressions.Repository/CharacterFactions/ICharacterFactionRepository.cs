using ExpressedRealms.DB.Models.Factions.CharacterFactionMappingModels;
using ExpressedRealms.Expressions.Repository.CharacterFactions.Dtos;

namespace ExpressedRealms.Expressions.Repository.CharacterFactions;

public interface ICharacterFactionRepository
{
    Task<int> JoinFaction(CharacterFactionMapping characterFactionMapping);
    Task<List<CharacterFactionDto>> GetLatestPlayerFactionLevels(int characterId);
    Task<PlayerFactionInfoDto?> GetPlayerFactionInfo(int characterId);
    Task<List<BasicFactionLevelProjection>> GetFactionLevels(int characterId);
    Task<List<CharacterFactionMapping>> GetFactionLevelsForBulkEditing(int characterId);
    Task BulkEditCharacterFactionAsync(List<CharacterFactionMapping> factionMappings);
}
