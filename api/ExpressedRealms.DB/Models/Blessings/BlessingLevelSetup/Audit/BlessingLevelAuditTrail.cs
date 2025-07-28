using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Blessings.BlessingSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

namespace ExpressedRealms.DB.Models.Blessings.BlessingLevelSetup.Audit;

public class BlessingLevelAuditTrail : IAuditTable
{
    public int BlessingId { get; set; }
    public virtual Blessing Blessing { get; set; } = null!;

    public int BlessingLevelId { get; set; }
    public virtual BlessingLevel BlessingLevel { get; set; } = null!;

    public int Id { get; set; }
    public required string Action { get; set; }
    public DateTime Timestamp { get; set; }
    public required string ActorUserId { get; set; }
    public required string ChangedProperties { get; set; }
    public virtual User ActorUser { get; set; } = null!;
}
