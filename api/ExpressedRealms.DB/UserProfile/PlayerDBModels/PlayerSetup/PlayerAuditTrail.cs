using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

namespace ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerSetup;

public class PlayerAuditTrail : IAuditTable
{
    public Guid PlayerId { get; set; }
    public int Id { get; set; }
    public string Action { get; set; }
    public DateTime Timestamp { get; set; }
    public string UserId { get; set; }
    public string ChangedProperties { get; set; }

    public virtual User User { get; set; }
    public virtual Player Player { get; set; }
}
