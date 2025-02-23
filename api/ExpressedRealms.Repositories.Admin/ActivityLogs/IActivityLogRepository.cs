using ExpressedRealms.Repositories.Admin.DTOs;

namespace ExpressedRealms.Repositories.Admin;

public interface IActivityLogRepository
{
    Task<List<Log>> GetUserLogs(string userId);
}
