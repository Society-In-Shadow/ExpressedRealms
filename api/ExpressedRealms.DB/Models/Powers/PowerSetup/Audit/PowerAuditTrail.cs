using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Powers.PowerPathSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

namespace ExpressedRealms.DB.Models.Powers.PowerSetup.Audit;

public class PowerAuditTrail : IAuditTable
{
    public int PowerPathId { get; set; }
    public int PowerId { get; set; }

    public int Id { get; set; }
    public string Action { get; set; }
    public DateTime Timestamp { get; set; }
    public string ActorUserId { get; set; }
    public string ChangedProperties { get; set; }

    public virtual PowerPath PowerPath { get; set; }
    public virtual Power Power { get; set; }
    public virtual User ActorUser { get; set; }
}
