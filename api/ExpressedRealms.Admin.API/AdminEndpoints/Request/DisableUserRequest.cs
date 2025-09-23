namespace ExpressedRealms.Admin.API.AdminEndpoints.Request;

public class DisableUserRequest
{
    public bool LockoutEnabled { get; set; }
    public string UserId { get; set; } = null!;
    public DateTime? CustomExpiryDate { get; set; }
}
