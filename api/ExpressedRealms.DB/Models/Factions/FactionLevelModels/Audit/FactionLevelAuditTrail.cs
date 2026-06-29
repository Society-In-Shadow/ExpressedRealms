using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

namespace ExpressedRealms.DB.Models.Factions.FactionLevelModels.Audit;

public class FactionLevelAuditTrail : IAuditTable
{
    public int FactionLevelId { get; set; }

    public int Id { get; set; }
    public required string Action { get; set; }
    public DateTime Timestamp { get; set; }
    public required string ActorUserId { get; set; }
    public required string ChangedProperties { get; set; }

    public virtual FactionLevel FactionLevel { get; set; } = null!;
    public virtual User ActorUser { get; set; } = null!;
}
