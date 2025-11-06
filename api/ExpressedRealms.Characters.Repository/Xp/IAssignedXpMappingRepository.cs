using ExpressedRealms.DB.Characters.AssignedXp.AssignedXpMappingModels;
using ExpressedRealms.Shared;

namespace ExpressedRealms.Characters.Repository.Xp;

public interface IAssignedXpMappingRepository : IGenericRepository
{
    Task<int> AddAsync(AssignedXpMapping entity);
    Task<TEntity?> FindAsync<TEntity>(int id)
        where TEntity : class;

    Task EditAsync<TEntity>(TEntity entity)
        where TEntity : class;
}
