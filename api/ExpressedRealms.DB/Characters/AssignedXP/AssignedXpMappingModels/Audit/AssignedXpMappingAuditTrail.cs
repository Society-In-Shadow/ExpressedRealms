using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

namespace ExpressedRealms.DB.Characters.AssignedXp.AssignedXpMappingModels.Audit;

public class AssignedXpMappingAuditTrail : IAuditTable
{
    public int AssignedXpMappingId { get; set; }

    public virtual AssignedXpMapping AssignedXpMapping { get; set; } = null!;
    
    public int Id { get; set; }
    public required string Action { get; set; }
    public DateTime Timestamp { get; set; }
    public required string ActorUserId { get; set; }
    public required string ChangedProperties { get; set; }
    public virtual User ActorUser { get; set; } = null!;
}
