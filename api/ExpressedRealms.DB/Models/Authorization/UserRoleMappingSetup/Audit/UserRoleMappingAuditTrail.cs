using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Authorization.RoleSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

namespace ExpressedRealms.DB.Models.Authorization.UserRoleMappingSetup.Audit;

public class UserRoleMappingAuditTrail : IAuditTable
{
    public int UserRoleMappingId { get; set; }
    public virtual UserRoleMapping UserRoleMapping { get; set; } = null!;
    
    public int RoleId { get; set; }
    public virtual Role Role { get; set; } = null!;
    
    public int UserId { get; set; }
    public virtual User User { get; set; } = null!;
    
    public int Id { get; set; }
    public required string Action { get; set; }
    public DateTime Timestamp { get; set; }
    public required string ActorUserId { get; set; }
    public required string ChangedProperties { get; set; }
    public virtual User ActorUser { get; set; } = null!;
}
