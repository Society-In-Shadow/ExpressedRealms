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

    public async Task<int?> GetActiveEventId()
    {
        var now = DateOnly.FromDateTime(DateTime.UtcNow);

        var eventId = await context.Events.AsNoTracking()
            .Where(x => x.IsPublished && x.StartDate <= now && x.EndDate >= now)
            .Select(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);
        
        return eventId == 0 ? null : eventId;
    }

    public Task EditAsync<TEntity>(TEntity entity) where TEntity : class
    {
        throw new NotImplementedException();
    }
}
