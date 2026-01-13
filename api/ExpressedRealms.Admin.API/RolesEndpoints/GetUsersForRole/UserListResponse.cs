namespace ExpressedRealms.Admin.API.RolesEndpoints.GetUsersForRole;

public class RoleListResponse
{
    public List<UserMapping> Roles { get; set; } = new();
}
