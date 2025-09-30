using ExpressedRealms.DB;
using ExpressedRealms.DB.Models.ModifierSystem.StatGroupMappings;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Expressions.Repository.StatModifier;

public class StatModifierRepository(
    ExpressedRealmsDbContext context,
    CancellationToken cancellationToken
) : IStatModifierRepository
{
    public async Task<StatGroupMapping> GetGroupMappingForEditing(int id)
    {
        return await context.StatGroupMappings.FirstAsync(x => x.Id == id);
    }

    public Task HardDeleteGroupMapping(StatGroupMapping mapping)
    {
        context.StatGroupMappings.Remove(mapping);
        return context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> GroupMappingExists(int groupId, int id)
    {
        return await context.StatGroupMappings.AnyAsync(x => x.StatGroupId == groupId && x.Id == id);
    }
}
