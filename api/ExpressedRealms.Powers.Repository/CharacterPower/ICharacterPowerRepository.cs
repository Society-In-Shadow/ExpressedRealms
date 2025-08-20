using ExpressedRealms.DB.Models.Powers.CharacterPowerMappingSetup;

namespace ExpressedRealms.Powers.Repository.CharacterPower;

public interface ICharacterPowerRepository
{
    Task<bool> MappingExistsAsync(int powerId, int characterId);
    Task<int> GetExperienceSpentOnPowersForCharacter(int modelCharacterId);
    Task<int> AddCharacterPowerMapping(CharacterPowerMapping characterPowerMapping);
    Task<List<int>> GetSelectablePowersForCharacter(int characterId);
}
