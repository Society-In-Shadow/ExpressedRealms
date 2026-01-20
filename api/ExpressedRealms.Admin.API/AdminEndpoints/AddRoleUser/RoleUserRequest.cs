namespace ExpressedRealms.Admin.API.AdminEndpoints.AddRoleUser;

public class RoleUserRequest
{
    public int RoleId { get; set; }
    public DateOnly? ExpireDate { get; set; }
}
