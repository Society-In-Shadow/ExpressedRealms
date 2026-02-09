using ExpressedRealms.Shared;

namespace ExpressedRealms.Events.API.Repositories.EventCheckin;

public interface IEventCheckinRepository : IGenericRepository
{
    Task<string> GetPlayerLookupId();
    Task<int?> GetActiveEventId();
}
