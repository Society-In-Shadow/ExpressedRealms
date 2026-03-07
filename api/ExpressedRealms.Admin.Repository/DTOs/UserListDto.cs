namespace ExpressedRealms.Admin.Repository.DTOs;

public class UserListDto
{
    public required string Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public List<RoleInfoDto> Roles { get; set; } = new ();
    public List<string?> LegacyRoles { get; set; } = new List<string?>();
    public bool IsDisabled { get; set; }
    public bool LockedOut { get; set; }
    public DateTimeOffset? LockOutExpires { get; set; }
    public bool EmailConfirmed { get; set; }
}
