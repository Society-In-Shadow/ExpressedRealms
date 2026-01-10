namespace ExpressedRealms.Admin.UseCases.Roles.GetPermissions;

public class PermissionsBaseReturnModel
{
    public List<ResourceGroup> Resources { get; set; } = new();
}
