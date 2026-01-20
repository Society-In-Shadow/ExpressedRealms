using ExpressedRealms.Admin.Repository.ActivityLogs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Admin.API.AdminEndpoints.ViewActivityLogs;

public static class ViewActivityLogsEndpoint
{
    public static async Task<Results<Ok<LogResponse>, NotFound>> Execute(
        Guid userId,
        IActivityLogRepository repository
    )
    {
        var userLogs = await repository.GetUserLogs(userId.ToString());

        return TypedResults.Ok(
            new LogResponse()
            {
                Logs = userLogs
                    .Select(
                        (x, index) =>
                            new LogDto()
                            {
                                Id = index,
                                ChangedProperties = x.ChangedProperties,
                                Location = x.Location,
                                TimeStamp = x.TimeStamp,
                                Action = x.Action,
                            }
                    )
                    .ToList(),
            }
        );
    }
}
