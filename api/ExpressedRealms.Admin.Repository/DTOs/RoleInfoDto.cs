namespace ExpressedRealms.Admin.Repository.DTOs;

public class RoleInfoDto
{
    public required string Name { get; set; }
    public DateOnly? ExpirationDate { get; set; }
}
