namespace ExpressedRealms.Admin.UseCases.Roles.AddUserToRole;

public class AddUserToRoleModel
{
    public required string UserId { get; set; }
    public int RoleId { get; set; }
    public DateOnly? ExpireDate { get; set; }
}
