using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Authorization.Permissions;
using ExpressedRealms.DB.Models.Authorization.RoleSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

namespace ExpressedRealms.DB.Models.Authorization.RolePermissionMappingSetup.Audit;

public class RolePermissionMappingAuditTrail : IAuditTable
{
    public int RolePermissionMappingId { get; set; }
    public virtual RolePermissionMapping RolePermissionMapping { get; set; } = null!;
    
    public int RoleId { get; set; }
    public virtual Role Role { get; set; } = null!;
    
    public Guid? PermissionId { get; set; }
    public virtual Permission Permission { get; set; } = null!;
    
    public int Id { get; set; }
    public required string Action { get; set; }
    public DateTime Timestamp { get; set; }
    public required string ActorUserId { get; set; }
    public required string ChangedProperties { get; set; }
    public virtual User ActorUser { get; set; } = null!;
}
