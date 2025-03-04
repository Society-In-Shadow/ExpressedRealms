using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.Roles;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

namespace ExpressedRealms.DB.UserProfile.PlayerDBModels.UserRoles;

public class UserRoleAuditTrail : IAuditTable
{
    public string RoleId { get; set; }
    public string MappingUserId { get; set; }

    public int Id { get; set; }
    public string Action { get; set; }
    public DateTime Timestamp { get; set; }
    public string ActorUserId { get; set; }
    public string ChangedProperties { get; set; }

    public virtual User ActorUser { get; set; }
    public virtual User MappingUser { get; set; }
    public virtual Role Role { get; set; }
}
