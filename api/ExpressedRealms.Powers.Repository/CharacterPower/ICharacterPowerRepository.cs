using ExpressedRealms.DB.Models.Powers.CharacterPowerMappingSetup;
using ExpressedRealms.Powers.Repository.CharacterPower.DTO;

namespace ExpressedRealms.Powers.Repository.CharacterPower;

public interface ICharacterPowerRepository
{
    Task<bool> MappingExistsAsync(int powerId, int characterId);
    Task<int> GetExperienceSpentOnPowersForCharacter(int characterId);
    Task<int> AddCharacterPowerMapping(CharacterPowerMapping characterPowerMapping);
    Task<List<int>> GetSelectablePowersForCharacter(int characterId);
    Task<CharacterPowerMapping> GetCharacterPowerMapping(int characterId, int powerId);
    Task UpdateCharacterPowerMapping(CharacterPowerMapping characterPowerMapping);
    Task<bool> IsValidMapping(int id);
    Task<List<CharacterPowerInfo>> GetCharacterPowerMappingInfo(int characterId);
}
