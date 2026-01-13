namespace ExpressedRealms.Admin.UseCases.Users.GetUsersForRole;

public class RoleForUserMappingReturnModel
{
    public required string UserId { get; set; }
    public required string Name { get; set; }
    public DateOnly? ExpireDate { get; set; }
}
