using ExpressedRealms.DB;
using ExpressedRealms.Repositories.Admin.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Repositories.Admin;

public class ActivityLogRepository(ExpressedRealmsDbContext context) : IActivityLogRepository
{
    public async Task<List<Log>> GetUserLogs(string userId)
    {
        var expressionLogs = await context
            .ExpressionAuditTrails.AsNoTracking()
            .IgnoreQueryFilters()
            .Where(x => x.UserId == userId)
            .Select(x => new Log()
            {
                Location = $"Expression \"{x.Expression.Name}\"",
                TimeStamp = x.Timestamp,
                Action = x.Action,
                ChangedProperties = x.ChangedProperties,
            })
            .ToListAsync();

        var expressionSectionsLogs = await context
            .ExpressionSectionAuditTrails.AsNoTracking()
            .IgnoreQueryFilters()
            .Where(x => x.UserId == userId)
            .Select(x => new Log()
            {
                Location =
                    $"Expression \"{x.Expression.Name}\" > Expression Section \"{x.ExpressionSection.Name}\"",
                TimeStamp = x.Timestamp,
                Action = x.Action,
                ChangedProperties = x.ChangedProperties,
            })
            .ToListAsync();

        var userLogs = await context
            .UserAuditTrails.AsNoTracking()
            .IgnoreQueryFilters()
            .Where(x => x.UserId == userId)
            .Select(x => new Log()
            {
                Location = $"Player \"{x.User.Player.Name}\"",
                TimeStamp = x.Timestamp,
                Action = x.Action,
                ChangedProperties = x.ChangedProperties,
            })
            .ToListAsync();

        var playerLogs = await context
            .PlayerAuditTrails.AsNoTracking()
            .IgnoreQueryFilters()
            .Where(x => x.UserId == userId)
            .Select(x => new Log()
            {
                Location = $"Player \"{x.Player.Name}\"",
                TimeStamp = x.Timestamp,
                Action = x.Action,
                ChangedProperties = x.ChangedProperties,
            })
            .ToListAsync();

        var userRoleLogs = await context
            .UserRoleAuditTrails.AsNoTracking()
            .IgnoreQueryFilters()
            .Where(x => x.UserId == userId)
            .Select(x => new Log()
            {
                Location = $"Role \"{x.Role.Name}\" for Player \"{x.MappingUser.Player.Name}\"",
                TimeStamp = x.Timestamp,
                Action = x.Action,
                ChangedProperties = x.ChangedProperties,
            })
            .ToListAsync();

        return expressionLogs
            .Concat(expressionSectionsLogs)
            .Concat(userLogs)
            .Concat(playerLogs)
            .Concat(userRoleLogs)
            .ToList();
    }
}
