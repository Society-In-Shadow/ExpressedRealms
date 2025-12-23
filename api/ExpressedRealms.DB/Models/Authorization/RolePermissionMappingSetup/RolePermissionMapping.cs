using Audit.EntityFramework;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Authorization.Permissions;
using ExpressedRealms.DB.Models.Authorization.RoleSetup;

namespace ExpressedRealms.DB.Models.Authorization.RolePermissionMappingSetup;

[AuditInclude]
public class RolePermissionMapping : ISoftDelete
{
    public int Id { get; set; }
    public int RoleId { get; set; }
    public int PermissionId { get; set; }
    public DateTimeOffset? ExpireDate { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public virtual Permission Permission { get; set; } = null!;
    public virtual Role Role { get; set; } = null!;
}
