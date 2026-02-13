using System.Linq.Expressions;
using ExpressedRealms.Characters.Repository.Xp.Dtos.AssignedXpInfoDtos;
using ExpressedRealms.DB;
using ExpressedRealms.DB.Characters.AssignedXp.AssignedXpMappingModels;
using ExpressedRealms.DB.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Characters.Repository.Xp;

public class AssignedXpMappingRepository(
    ExpressedRealmsDbContext context,
    CancellationToken cancellationToken
) : IAssignedXpMappingRepository
{
    public async Task<List<XpMappingInfoDto>> GetAllPlayerMappingsAsync(Guid playerId)
    {
        return await context
            .AssignedXpMappings.AsNoTracking()
            .Where(x => x.PlayerId == playerId)
            .Select(GetMappingInfoDto())
            .ToListAsync(cancellationToken);
    }

    public async Task<List<XpMappingInfoDto>> GetAllEventMappingsAsync(int eventId)
    {
        return await context
            .AssignedXpMappings.AsNoTracking()
            .Where(x => x.EventId == eventId)
            .Select(GetMappingInfoDto())
            .ToListAsync(cancellationToken);
    }

    private static Expression<Func<AssignedXpMapping, XpMappingInfoDto>> GetMappingInfoDto()
    {
        return x => new XpMappingInfoDto()
        {
            Id = x.Id,
            Amount = x.Amount,
            DateAssigned = x.Timestamp,
            Assigner = new BasicInfo() { Id = 0, Name = x.AssignedByUser.Player!.Name },
            Player = new BasicInfo() { Name = x.Player.Name, Id = 0 },
            Character = x.Character == null ? null : new BasicInfo
            {
                Id = x.Character.Id,
                Name = x.Character.Name
            },
            Event = new BasicInfo()
            {
                Name = $"{x.Event.Name} ({x.Event.StartDate.Year})",
                Id = x.EventId,
            },
            Notes = x.Reason,
            XpType = new BasicInfo() { Name = x.AssignedXpType.Name, Id = x.AssignedXpTypeId },
        };
    }

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
