using ExpressedRealms.Admin.API.RolesEndpoints.GetRoles;

namespace ExpressedRealms.Admin.API.RolesEndpoints.GetRole;

public class RoleRequest
{
    public List<Role> Roles { get; set; } = new List<Role>();
}
