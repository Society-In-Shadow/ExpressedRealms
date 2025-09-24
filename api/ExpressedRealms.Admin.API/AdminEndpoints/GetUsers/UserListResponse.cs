namespace ExpressedRealms.Admin.API.AdminEndpoints.GetUsers;

public class UserListResponse
{
    public List<UserListItem> Users { get; set; } = new();
}
