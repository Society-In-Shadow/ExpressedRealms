using ExpressedRealms.DB.Models.ModifierSystem.StatGroupMappings;

namespace ExpressedRealms.Expressions.Repository.StatModifier;

public interface IStatModifierRepository
{
    Task<StatGroupMapping> GetGroupMappingForEditing(int id);
    Task HardDeleteGroupMapping(StatGroupMapping mapping);
    Task<bool> GroupMappingExists(int groupId, int id);
}
