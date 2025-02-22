using ExpressedRealms.DB;
using ExpressedRealms.Repositories.Admin.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Repositories.Admin;

public class ActivityLogRepository(ExpressedRealmsDbContext context) : IActivityLogRepository
{
    public async Task<List<Log>> GetUserLogs(string userId)
    {
        var expressionLogs = await context.ExpressionAuditTrails.AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(x => new Log()
            {
                Location = x.Expression.Name,
                TimeStamp = x.Timestamp,
                Action = x.Action,
                ChangedProperties = x.ChangedProperties,
            })
            .ToListAsync();
        
        var expressionSectionsLogs = await context.ExpressionSectionAuditTrails.AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(x => new Log()
            {
                Location = $"{x.Expression.Name} > {x.ExpressionSection.Name}",
                TimeStamp = x.Timestamp,
                Action = x.Action,
                ChangedProperties = x.ChangedProperties,
            })
            .ToListAsync();
        
        return expressionLogs.Concat(expressionSectionsLogs).ToList();
    }
}