using ExpressedRealms.DB.UserProfile.PlayerDBModels;

namespace ExpressedRealms.DB.Interceptors;

public interface IAuditTable
{
    public int Id { get; set; }
    public string Action { get; set; }
    public DateTime Timestamp { get; set; }
    public Guid UserId { get; set; }
    public string ChangedProperties { get; set; }
}