namespace ExpressedRealms.Server.EndPoints.AdminEndpoints.Request;

public class UpdateUserRoleRequest
{
    public string UserId { get; set; } = null!;
    public string RoleName { get; set; } = null!;
    public bool IsEnabled { get; set; }
}
