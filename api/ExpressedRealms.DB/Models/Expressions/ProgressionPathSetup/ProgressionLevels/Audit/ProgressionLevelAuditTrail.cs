using Audit.EntityFramework;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionPaths;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

namespace ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionLevels.Audit;

[AuditInclude]
public class ProgressionLevelAuditTrail : IAuditTable
{
    public int ProgressionLevelId { get; set; }
    public int ProgressionPathId { get; set; }

    public virtual ProgressionPath ProgressionPath { get; set; } = null!;
    public virtual ProgressionLevel ProgressionLevel { get; set; } = null!;

    public int Id { get; set; }
    public required string Action { get; set; }
    public DateTime Timestamp { get; set; }
    public required string ActorUserId { get; set; }
    public required string ChangedProperties { get; set; }
    public virtual User ActorUser { get; set; } = null!;
}
