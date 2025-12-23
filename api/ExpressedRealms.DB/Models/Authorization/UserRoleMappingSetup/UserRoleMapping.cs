using Audit.EntityFramework;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Authorization.RoleSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

namespace ExpressedRealms.DB.Models.Authorization.UserRoleMappingSetup;

[AuditInclude]
public class UserRoleMapping : ISoftDelete
{
    public int Id { get; set; }
    public int RoleId { get; set; }
    public int UserId { get; set; }
    public DateTimeOffset? ExpireDate { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public virtual User User { get; set; } = null!;
    public virtual Role Role { get; set; } = null!;
}
