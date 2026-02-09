using ExpressedRealms.Shared;

namespace ExpressedRealms.Events.API.Repositories.EventCheckin;

public interface IEventCheckinRepository : IGenericRepository
{
    Task<string> GetPlayerLookupId();
    Task<int?> GetActiveEventId();
    Task<bool> CheckinIdExistsAsync(string id);
    Task<string> GetUserName(string lookupId);
    Task<bool> IsFirstTimePlayer(string lookupId);
}
