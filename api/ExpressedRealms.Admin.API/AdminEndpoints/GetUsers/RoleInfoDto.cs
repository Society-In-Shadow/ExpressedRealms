namespace ExpressedRealms.Admin.API.AdminEndpoints.GetUsers;

public class RoleInfoDto
{
    public string Name { get; set; }
    public DateOnly? ExpirationDate { get; set; }
}
