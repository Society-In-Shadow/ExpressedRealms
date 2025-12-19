using ExpressedRealms.DB.Models.Authorization.Permissions;

namespace ExpressedRealms.DB.Models.Authorization.PermissionResources;

public class PermissionResource
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }

    public virtual ICollection<Permission> Permissions { get; set; } = [];
}
