namespace ExpressedRealms.Server.EndPoints.AuthEndpoints.UserEndpoint;

public class UserInfo
{
    public string? Name { get; set; } = null!;
    public SetupState SetupState { get; set; }
    public string? Email { get; set; }
}
