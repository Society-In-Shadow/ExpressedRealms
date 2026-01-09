namespace ExpressedRealms.Admin.API.RolesEndpoints.GetRole;

public class RoleResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public List<Guid> PermissionIds { get; set; } = new List<Guid>();
}
