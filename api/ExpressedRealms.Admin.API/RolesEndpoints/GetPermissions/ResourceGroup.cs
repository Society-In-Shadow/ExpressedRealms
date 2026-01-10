namespace ExpressedRealms.Admin.API.RolesEndpoints.GetPermissions;

public class ResourceGroup
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public List<Permission> Permissions { get; set; } = new();
}
