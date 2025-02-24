using ExpressedRealms.DB.Interceptors;

namespace ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

public class UserAuditTrail : IAuditTable
{
    public bool LoggedIn { get; set; }
    public bool LoggedOut { get; set; }
    public bool PasswordChanged { get; set; }
    public bool EmailChanged { get; set; }
    
    public int Id { get; set; }
    public string Action { get; set; }
    public DateTime Timestamp { get; set; }
    public string UserId { get; set; }
    public string ChangedProperties { get; set; }
    
    public virtual User User { get; set; }
}