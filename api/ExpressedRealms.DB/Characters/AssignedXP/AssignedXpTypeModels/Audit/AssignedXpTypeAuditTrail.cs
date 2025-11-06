using ExpressedRealms.DB.Characters.AssignedXp.AssignedXpTypeModels;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

namespace ExpressedRealms.DB.Characters.AssignedXP.AssignedXpTypeModels.Audit;

public class AssignedXpTypeAuditTrail : IAuditTable
{
    public int AssignedXpTypeId { get; set; }

    public virtual AssignedXpType AssignedXpType { get; set; } = null!;

    public int Id { get; set; }
    public required string Action { get; set; }
    public DateTime Timestamp { get; set; }
    public required string ActorUserId { get; set; }
    public required string ChangedProperties { get; set; }
    public virtual User ActorUser { get; set; } = null!;
}
