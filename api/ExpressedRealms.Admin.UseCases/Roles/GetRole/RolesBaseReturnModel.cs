namespace ExpressedRealms.Admin.UseCases.Roles.GetRole;

public class RoleBaseReturnModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public List<Guid> PermissionIds { get; set; } = new();
}
