namespace ExpressedRealms.Admin.API.AdminEndpoints.Dtos;

public class UserListItem
{
    public string Id { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public List<string?> Roles { get; set; } = new List<string?>();
    public bool IsDisabled { get; set; }
    public bool LockedOut { get; set; }
    public DateTimeOffset? LockedOutExpires { get; set; }
    public bool EmailConfirmed { get; set; }
}
