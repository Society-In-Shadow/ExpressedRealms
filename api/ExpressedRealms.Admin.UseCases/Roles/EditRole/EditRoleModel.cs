namespace ExpressedRealms.Admin.UseCases.Roles.EditRole;

public class EditRoleModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public List<Guid> PermissionIds { get; set; } = new();
}
