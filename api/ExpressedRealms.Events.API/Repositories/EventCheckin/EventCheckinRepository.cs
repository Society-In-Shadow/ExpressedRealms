using ExpressedRealms.DB;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Events.API.Repositories.EventCheckin;

internal sealed class EventCheckinRepository(
    ExpressedRealmsDbContext context,
    IUserContext userContext,
    CancellationToken cancellationToken
) : IEventCheckinRepository
{

    public async Task<string> GetPlayerLookupId()
    {
        return await context.Players.AsNoTracking()
            .Where(x => x.UserId == userContext.CurrentUserId())
            .Select(x => x.LookupId!)
            .FirstAsync(cancellationToken);
    }

    public async Task<bool> CheckinIdExistsAsync(string id)
    {
        return await context.Players.AsNoTracking()
            .AnyAsync(x => x.LookupId == id, cancellationToken);
    }

    public async Task<int?> GetActiveEventId()
    {
        var now = DateOnly.FromDateTime(DateTime.UtcNow);

        var eventId = await context.Events.AsNoTracking()
            .Where(x => x.IsPublished && x.StartDate <= now && x.EndDate >= now)
            .Select(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);
        
        return eventId == 0 ? null : eventId;
    }

    public async Task<string> GetUserName(string lookupId)
    {
        return await context.Players
            .Where(x => x.LookupId == lookupId)
            .Select(x => x.Name)
            .FirstAsync(cancellationToken);
    }

    public async Task<bool> IsFirstTimePlayer(string lookupId)
    {
        const int firstTimePlayerBonus = 4;
        return await context.AssignedXpMappings.AsNoTracking()
            .AnyAsync(x => x.Player.LookupId == lookupId && 
                           x.AssignedXpTypeId == firstTimePlayerBonus, cancellationToken);
    }

    public Task EditAsync<TEntity>(TEntity entity) where TEntity : class
    {
        throw new NotImplementedException();
    }
}
