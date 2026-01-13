namespace ExpressedRealms.Admin.UseCases.Roles.GetRolesForUser;

public class RoleForUserMappingReturnModel
{
    public int RoleId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public DateOnly? ExpireDate { get; set; }
}
