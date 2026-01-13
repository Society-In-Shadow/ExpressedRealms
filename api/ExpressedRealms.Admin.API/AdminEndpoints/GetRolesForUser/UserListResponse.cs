namespace ExpressedRealms.Admin.API.AdminEndpoints.GetRolesForUser;

public class RoleListResponse
{
    public List<RoleMapping> Roles { get; set; } = new();
}
