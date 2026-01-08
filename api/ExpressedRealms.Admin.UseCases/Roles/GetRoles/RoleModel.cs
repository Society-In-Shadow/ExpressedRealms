namespace ExpressedRealms.Admin.UseCases.Roles.GetRoles;

public class RoleModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}
