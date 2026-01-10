namespace ExpressedRealms.Admin.API.RolesEndpoints.GetPermissions;

public class Permission
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}
