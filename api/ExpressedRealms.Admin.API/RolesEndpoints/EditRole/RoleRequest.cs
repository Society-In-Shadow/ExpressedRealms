namespace ExpressedRealms.Admin.API.RolesEndpoints.EditRole;

public class RoleRequest
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public List<Guid> PermissionIds { get; set; } = new();
}
