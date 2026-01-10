namespace ExpressedRealms.Admin.API.RolesEndpoints.GetPermissions;

public class PermissionsBaseResponse
{
    public List<ResourceGroup> Resources { get; set; } = new();
}
