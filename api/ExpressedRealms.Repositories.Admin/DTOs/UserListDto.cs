namespace ExpressedRealms.Repositories.Admin.DTOs;

public class UserListDto
{
    public string Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public List<string?> Roles { get; set; }
}
