using ExpressedRealms.Characters.Repository.Proficiencies.DTOs;
using ExpressedRealms.DB.Models.ModifierSystem.StatGroupMappings;

namespace ExpressedRealms.Expressions.Repository.StatModifier;

public interface IStatModifierRepository
{
    Task<StatGroupMapping> GetGroupMappingForEditing(int id);
    Task HardDeleteGroupMapping(StatGroupMapping mapping);
    Task<bool> GroupMappingExists(int groupId, int id);
    Task<bool> ModifierTypeExists(int id);
    Task<bool> GroupIdExists(int id);
    Task<bool> ProgressionPathExists(int id);
    Task<bool> PowerExists(int id);
    Task<bool> BlessingLevelExists(int id);
    Task UpdateBlessingGroupId(int blessingId, int groupId);
    Task UpdateProgressionPathGroupId(int progressionPathId, int groupId);
    Task UpdatePowerGroupId(int powerId, int groupId);
    Task UpdateGroupMapping(StatGroupMapping mapping);

    Task<int> AddGroup();
    Task<int> AddStatGroupMapping(StatGroupMapping mapping);
    Task<List<StatGroupMapping>> GetGroupMappings(int groupId);
    Task<List<DB.Models.ModifierSystem.StatModifiers.StatModifier>> GetModifierTypes();
    Task<bool> ProgressionLevelExists(int id);
    Task<List<ProficiencyModifierInfoDto>> GetModifiersFromBlessings(int characterId);
    Task<List<ProficiencyModifierInfoDto>> GetModifiersFromPowers(int characterId);
    Task<List<ProficiencyModifierInfoDto>> GetModifiersFromXlLevel(
        int characterId,
        int currentLevel
    );
}
