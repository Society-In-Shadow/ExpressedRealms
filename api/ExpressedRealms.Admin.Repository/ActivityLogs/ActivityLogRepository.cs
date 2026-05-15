using ExpressedRealms.Admin.Repository.ActivityLogs.DTOs;
using ExpressedRealms.DB;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Admin.Repository.ActivityLogs;

public class ActivityLogRepository(ExpressedRealmsDbContext context) : IActivityLogRepository
{
    public async Task<List<Log>> GetUserLogs(string userId)
    {
        var expressionLogs = await context
            .ExpressionAuditTrails.AsNoTracking()
            .IgnoreQueryFilters()
            .Where(x => x.ActorUserId == userId)
            .Select(x => new Log()
            {
                Location = $"{x.Expression.ExpressionType.Name} \"{x.Expression.Name}\"",
                TimeStamp = x.Timestamp,
                Action = x.Action,
                ChangedProperties = x.ChangedProperties,
            })
            .ToListAsync();

        var expressionSectionsLogs = await context
            .ExpressionSectionAuditTrails.AsNoTracking()
            .IgnoreQueryFilters()
            .Where(x => x.ActorUserId == userId)
            .Select(x => new Log()
            {
                Location =
                    $"{x.Expression.ExpressionType.Name} \"{x.Expression.Name}\" > Section \"{x.ExpressionSection.Name}\"",
                TimeStamp = x.Timestamp,
                Action = x.Action,
                ChangedProperties = x.ChangedProperties,
            })
            .ToListAsync();

        var userLogs = await context
            .UserAuditTrails.AsNoTracking()
            .IgnoreQueryFilters()
            .Where(x => x.ActorUserId == userId)
            .Select(x => new Log()
            {
                Location = $"Player \"{x.User.Player!.Name}\"",
                TimeStamp = x.Timestamp,
                Action = x.Action,
                ChangedProperties = x.ChangedProperties,
            })
            .ToListAsync();

        var userSpecificLogs = await context
            .UserAuditTrails.AsNoTracking()
            .IgnoreQueryFilters()
            .Where(x => x.UserId == userId && x.ActorUserId != userId)
            .Select(x => new Log()
            {
                Location =
                    $"Player \"{x.User.Player!.Name}\" was modified by \"{x.ActorUser.Player!.Name}\"",
                TimeStamp = x.Timestamp,
                Action = x.Action,
                ChangedProperties = x.ChangedProperties,
            })
            .ToListAsync();

        var playerLogs = await context
            .PlayerAuditTrails.AsNoTracking()
            .IgnoreQueryFilters()
            .Where(x => x.ActorUserId == userId)
            .Select(x => new Log()
            {
                Location = $"Player \"{x.Player.Name}\"",
                TimeStamp = x.Timestamp,
                Action = x.Action,
                ChangedProperties = x.ChangedProperties,
            })
            .ToListAsync();

        var playerSpecificLogs = await context
            .PlayerAuditTrails.AsNoTracking()
            .IgnoreQueryFilters()
            .Where(x => x.Player.UserId == userId && x.ActorUserId != userId)
            .Select(x => new Log()
            {
                Location =
                    $"Player \"{x.Player.Name}\" was modified by \"{x.ActorUser.Player!.Name}\"",
                TimeStamp = x.Timestamp,
                Action = x.Action,
                ChangedProperties = x.ChangedProperties,
            })
            .ToListAsync();

        var newRoleLogs = await context
            .RoleAuditTrails.AsNoTracking()
            .IgnoreQueryFilters()
            .Where(x => x.ActorUserId == userId)
            .Select(x => new Log()
            {
                Location = $"Role \"{x.Role.Name}\"",
                TimeStamp = x.Timestamp,
                Action = x.Action,
                ChangedProperties = x.ChangedProperties,
            })
            .ToListAsync();

        var newRolePermissionLogs = await context
            .RolePermissionMappingAuditTrails.AsNoTracking()
            .IgnoreQueryFilters()
            .Where(x => x.ActorUserId == userId)
            .Select(x => new Log()
            {
                Location =
                    $"Role \"{x.Role.Name}\" > "
                    + (
                        x.Permission == null
                            ? "Resource \"Removed\" > Permission \"Removed\""
                            : $"Resource \"{x.Permission!.Resource.Name}\" > Permission \"{x.Permission.Name}\""
                    ),
                TimeStamp = x.Timestamp,
                Action = x.Action,
                ChangedProperties = x.ChangedProperties,
            })
            .ToListAsync();

        var newActorUserRoleLogs = await context
            .UserRoleMappingAuditTrails.AsNoTracking()
            .IgnoreQueryFilters()
            .Where(x => x.ActorUserId == userId)
            .Select(x => new Log()
            {
                Location =
                    x.Action == "Insert"
                        ? $"Role \"{x.Role.Name}\" to Player \"{x.User.Player!.Name}\""
                        : $"Role \"{x.Role.Name}\" from Player \"{x.User.Player!.Name}\"",
                TimeStamp = x.Timestamp,
                Action = x.Action == "Insert" ? "Added" : "Removed",
                ChangedProperties = x.ChangedProperties,
            })
            .ToListAsync();

        var userSpecificNewRoleLogs = await context
            .UserRoleMappingAuditTrails.AsNoTracking()
            .IgnoreQueryFilters()
            .Where(x => x.UserId == userId && x.ActorUserId != userId)
            .Select(x => new Log()
            {
                Location =
                    x.Action == "Insert"
                        ? $"To Role \"{x.Role.Name}\" by \"{x.ActorUser.Player!.Name}\""
                        : $"From Role \"{x.Role.Name}\" by \"{x.ActorUser.Player!.Name}\"",
                TimeStamp = x.Timestamp,
                Action = x.Action == "Insert" ? "Added" : "Removed",
                ChangedProperties = x.ChangedProperties,
            })
            .ToListAsync();

        var powerPathLogs = await context
            .PowerPathAuditTrails.AsNoTracking()
            .IgnoreQueryFilters()
            .Where(x => x.ActorUserId == userId)
            .Select(x => new Log()
            {
                Location =
                    $"{x.Expression.ExpressionType.Name} \"{x.Expression.Name}\" > Power Path \"{x.PowerPath.Name}\"",
                TimeStamp = x.Timestamp,
                Action = x.Action,
                ChangedProperties = x.ChangedProperties,
            })
            .ToListAsync();

        var powerLogs = await context
            .PowerAuditTrails.AsNoTracking()
            .IgnoreQueryFilters()
            .Where(x => x.ActorUserId == userId)
            .Select(x => new Log()
            {
                Location =
                    $"{x.PowerPath.Expression.ExpressionType.Name} \"{x.Power.PowerPath.Expression.Name}\" > Power Path \"{x.PowerPath.Name}\" > Power \"{x.Power.Name}\"",
                TimeStamp = x.Timestamp,
                Action = x.Action,
                ChangedProperties = x.ChangedProperties,
            })
            .ToListAsync();

        var knowledgeLogs = await context
            .KnowledgeAuditTrails.AsNoTracking()
            .IgnoreQueryFilters()
            .Where(x => x.ActorUserId == userId)
            .Select(x => new Log()
            {
                Location = $"Knowledge \"{x.Knowledge.Name}\"",
                TimeStamp = x.Timestamp,
                Action = x.Action,
                ChangedProperties = x.ChangedProperties,
            })
            .ToListAsync();

        return expressionLogs
            .Concat(expressionSectionsLogs)
            .Concat(userLogs)
            .Concat(userSpecificLogs)
            .Concat(playerLogs)
            .Concat(playerSpecificLogs)
            .Concat(powerPathLogs)
            .Concat(powerLogs)
            .Concat(knowledgeLogs)
            .Concat(newRoleLogs)
            .Concat(newRolePermissionLogs)
            .Concat(newActorUserRoleLogs)
            .Concat(userSpecificNewRoleLogs)
            .ToList();
    }
}
