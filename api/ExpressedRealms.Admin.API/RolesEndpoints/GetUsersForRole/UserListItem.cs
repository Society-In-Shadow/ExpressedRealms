namespace ExpressedRealms.Admin.API.RolesEndpoints.GetUsersForRole;

public class UserMapping
{
    public required string UserId { get; set; }
    public required string Name { get; set; }
    public DateOnly? ExpireDate { get; set; }
}
