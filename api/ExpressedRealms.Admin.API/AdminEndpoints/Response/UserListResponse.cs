using ExpressedRealms.Admin.API.AdminEndpoints.Dtos;

namespace ExpressedRealms.Admin.API.AdminEndpoints.Response;

public class UserListResponse
{
    public List<UserListItem> Users { get; set; } = new();
}
