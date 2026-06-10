using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

namespace ExpressedRealms.DB.Models.Factions.FactionModels.Audit;

public class FactionAuditTrail : IAuditTable
{
    public int FactionId { get; set; }

    public int Id { get; set; }
    public required string Action { get; set; }
    public DateTime Timestamp { get; set; }
    public required string ActorUserId { get; set; }
    public required string ChangedProperties { get; set; }

    public virtual Faction Faction { get; set; } = null!;
    public virtual User ActorUser { get; set; } = null!;
}
