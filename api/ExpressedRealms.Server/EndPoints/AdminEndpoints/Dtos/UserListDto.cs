namespace ExpressedRealms.Server.EndPoints.AdminEndpoints.Dtos;

public class UserListItem
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
}
