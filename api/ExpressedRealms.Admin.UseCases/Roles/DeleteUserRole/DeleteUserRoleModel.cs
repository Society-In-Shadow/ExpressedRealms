namespace ExpressedRealms.Admin.UseCases.Roles.DeleteUserRole;

public class DeleteUserRoleModel
{
    public required string UserId { get; set; }
    public int RoleId { get; set; }
}
