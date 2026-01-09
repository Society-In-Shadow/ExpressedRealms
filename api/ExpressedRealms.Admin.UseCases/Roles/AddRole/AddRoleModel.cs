namespace ExpressedRealms.Admin.UseCases.Roles.AddRole;

public class AddRoleModel
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public List<Guid> PermissionIds { get; set; } = new();
}
