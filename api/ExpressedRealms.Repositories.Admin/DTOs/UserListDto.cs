namespace ExpressedRealms.Repositories.Admin.DTOs;

public class UserListDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
}
