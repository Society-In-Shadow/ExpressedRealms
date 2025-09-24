using ExpressedRealms.Admin.Repository.ActivityLogs.DTOs;

namespace ExpressedRealms.Admin.Repository.ActivityLogs;

public interface IActivityLogRepository
{
    Task<List<Log>> GetUserLogs(string userId);
}
