using ExpressedRealms.Characters.Repository.Xp.Dtos.AssignedXpInfoDtos;
using ExpressedRealms.DB.Characters.AssignedXp.AssignedXpMappingModels;
using ExpressedRealms.Shared;

namespace ExpressedRealms.Characters.Repository.Xp;

public interface IAssignedXpMappingRepository : IGenericRepository
{
    Task<int> AddAsync(AssignedXpMapping entity);
    Task<TEntity?> FindAsync<TEntity>(int id)
        where TEntity : class;
    Task<List<XpMappingInfoDto>> GetAllPlayerMappingsAsync(Guid playerId);
    Task<List<XpMappingInfoDto>> GetAllEventMappingsAsync(int eventId);
}
