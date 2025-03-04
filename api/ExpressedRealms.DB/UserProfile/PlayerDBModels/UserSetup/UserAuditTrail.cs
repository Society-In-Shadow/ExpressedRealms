using ExpressedRealms.DB.Interceptors;

namespace ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

public class UserAuditTrail : IAuditTable
{
    public string UserId { get; set; }
    public int Id { get; set; }
    public string Action { get; set; }
    public DateTime Timestamp { get; set; }
    public string ActorUserId { get; set; }
    public string ChangedProperties { get; set; }

    public virtual User ActorUser { get; set; }
    public virtual User User { get; set; }
}
