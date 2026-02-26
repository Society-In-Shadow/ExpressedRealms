using ExpressedRealms.Characters.Repository.Xp.Dtos;
using ExpressedRealms.DB.Models.Characters.XpTables;

namespace ExpressedRealms.Characters.Repository.Xp;

public interface IXpRepository
{
    Task<CharacterXpView> GetCharacterXpMapping(int characterId, int sectionTypeId);
    Task<int> GetAvailableDiscretionary(int characterId);
    Task UpdateXpInfo(CharacterXpMapping xpInfo);
    Task<List<CharacterXpView>> GetCharacterXpMappings(int characterId);
    Task AddDefaultCharacterXpMappings(int characterId);
    Task<SectionXpDto> GetAvailableXpForSection(int characterId, XpSectionTypes sectionType);
    Task FinalizeCreateXp(int charactterId);
    Task<int> GetCharacterXpLevel(int characterId);
}
