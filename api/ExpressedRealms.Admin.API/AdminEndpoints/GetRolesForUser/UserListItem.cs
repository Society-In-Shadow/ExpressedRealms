namespace ExpressedRealms.Admin.API.AdminEndpoints.GetRolesForUser;

public class RoleMapping
{
    public int RoleId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public DateOnly? ExpireDate { get; set; }
}
