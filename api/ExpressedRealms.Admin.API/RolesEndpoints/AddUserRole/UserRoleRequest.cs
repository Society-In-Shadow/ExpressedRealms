namespace ExpressedRealms.Admin.API.RolesEndpoints.AddUserRole;

public class UserRoleRequest
{
    public required string UserId { get; set; }
    public DateOnly? ExpireDate { get; set; }
}
