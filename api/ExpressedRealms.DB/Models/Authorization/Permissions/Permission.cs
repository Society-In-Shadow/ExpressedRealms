using ExpressedRealms.DB.Models.Authorization.PermissionResources;
using ExpressedRealms.DB.Models.Authorization.RolePermissionMappingSetup;

namespace ExpressedRealms.DB.Models.Authorization.Permissions;

public class Permission
{
    public Guid Id { get; set; }
    public Guid PermissionResourceId { get; set; }
    public required string Key { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }

    public virtual PermissionResource Resource { get; set; } = null!;
    public virtual ICollection<RolePermissionMapping> RolePermissionMappings { get; set; } = new List<RolePermissionMapping>();
}
