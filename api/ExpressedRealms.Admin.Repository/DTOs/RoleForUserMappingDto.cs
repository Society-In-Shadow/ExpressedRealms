namespace ExpressedRealms.Admin.Repository.DTOs;

public class RoleForUserMappingDto
{
    public int RoleId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public DateOnly? ExpireDate { get; set; }
}
