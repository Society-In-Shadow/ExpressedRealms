using ExpressedRealms.DB.Characters.xpTables;

namespace ExpressedRealms.Characters.Repository.Xp;

public interface IXpRepository
{
    Task<CharacterXpMapping> GetCharacterXpMapping(int characterId, int sectionTypeId);
    Task<int> GetAvailableDiscretionary(int characterId);
    Task UpdateXpInfo(CharacterXpMapping xpInfo);
}