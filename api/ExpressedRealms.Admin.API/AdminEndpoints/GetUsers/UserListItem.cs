namespace ExpressedRealms.Admin.API.AdminEndpoints.GetUsers;

public class UserListItem
{
    public string Id { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public List<RoleInfoDto> Roles { get; set; } = new ();
    public bool IsDisabled { get; set; }
    public bool LockedOut { get; set; }
    public DateTimeOffset? LockedOutExpires { get; set; }
    public bool EmailConfirmed { get; set; }
    public List<string?> LegacyRoles { get; set; } = new();
}
