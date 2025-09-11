using ExpressedRealms.DB.Models.Blessings.CharacterBlessingMappings;

namespace ExpressedRealms.Blessings.Repository.CharacterBlessings;

public interface ICharacterBlessingRepository
{
    Task<bool> MappingAlreadyExists(int blessingId, int characterId);
    Task<int> GetExperienceSpentOnBlessingsForCharacter(int characterId);
    Task<int> AddCharacterBlessingMapping(CharacterBlessingMapping characterBlessingMapping);
    Task<int> GetExperienceAvailableToSpendOnCharacter(int characterId);
}