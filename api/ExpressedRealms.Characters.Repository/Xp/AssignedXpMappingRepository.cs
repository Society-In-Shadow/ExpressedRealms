using ExpressedRealms.DB;
using ExpressedRealms.DB.Characters.AssignedXp.AssignedXpMappingModels;
using ExpressedRealms.DB.Helpers;

namespace ExpressedRealms.Characters.Repository.Xp;

public class AssignedXpMappingRepository(
    ExpressedRealmsDbContext context,
    CancellationToken cancellationToken
) : IAssignedXpMappingRepository
{
    public async Task<int> AddAsync(AssignedXpMapping entity)
    {
        context.AssignedXpMappings.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public async Task<TEntity?> FindAsync<TEntity>(int id)
        where TEntity : class
    {
        return await context.Set<TEntity>().FindAsync([id], cancellationToken);
    }

    public async Task EditAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        await context.CommonSaveChanges(entity, cancellationToken);
    }
}
