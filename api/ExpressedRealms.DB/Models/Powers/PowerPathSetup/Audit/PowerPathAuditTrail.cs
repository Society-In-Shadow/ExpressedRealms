using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Expressions.ExpressionSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

namespace ExpressedRealms.DB.Models.Powers.PowerPathSetup;

public class PowerPathAuditTrail : IAuditTable
{
    public int ExpressionId { get; set; }
    public int PowerPathId { get; set; }

    public int Id { get; set; }
    public string Action { get; set; }
    public DateTime Timestamp { get; set; }
    public string ActorUserId { get; set; }
    public string ChangedProperties { get; set; }

    public virtual Expression Expression { get; set; }
    public virtual PowerPath PowerPath { get; set; }
    public virtual User ActorUser { get; set; }
}
