namespace ExpressedRealms.Admin.Repository.DTOs;

public class UserForRoleMappingDto
{
    public required string UserId { get; set; }
    public required string Name { get; set; }
    public DateOnly? ExpireDate { get; set; }
}
