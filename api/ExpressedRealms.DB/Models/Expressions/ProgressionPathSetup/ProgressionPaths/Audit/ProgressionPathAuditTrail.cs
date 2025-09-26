using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Expressions.ExpressionSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

namespace ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionPaths.Audit;

public class ProgressionPathAuditTrail : IAuditTable
{
    public int ExpressionId { get; set; }
    public int ProgressionPathId { get; set; }
    
    public virtual ProgressionPath ProgressionPath { get; set; } = null!;
    public virtual Expression Expression { get; set; } = null!;
    
    public int Id { get; set; }
    public required string Action { get; set; }
    public DateTime Timestamp { get; set; }
    public required string ActorUserId { get; set; }
    public required string ChangedProperties { get; set; }
    public virtual User ActorUser { get; set; } = null!;
}
