using ExpressedRealms.Server.EndPoints.AdminEndpoints.Dtos;

namespace ExpressedRealms.Server.EndPoints.AdminEndpoints.Response;

public class UserListResponse
{
    public List<UserListItem> Users { get; set; } = new();
}
