using ExpressedRealms.DB.Interceptors;

namespace ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

public class UserAuditTrail : IAuditTable
{
    public int Id { get; set; }
    public string Action { get; set; }
    public DateTime Timestamp { get; set; }
    public string UserId { get; set; }
    public string ChangedProperties { get; set; }

    public virtual User User { get; set; }
}
