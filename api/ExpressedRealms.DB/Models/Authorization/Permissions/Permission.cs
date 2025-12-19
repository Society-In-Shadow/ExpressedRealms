using ExpressedRealms.DB.Models.Authorization.PermissionResources;

namespace ExpressedRealms.DB.Models.Authorization.Permissions;

public class Permission
{
    public int Id { get; set; }
    public int PermissionResourceId { get; set; }
    public required string Key { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }

    public virtual PermissionResource Resource { get; set; } = null!;
}
