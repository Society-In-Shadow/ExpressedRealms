namespace ExpressedRealms.Authentication.PermissionCollection.Support;

public record Permission(string Resource, string Action) : IPermissionAction
{
    public string ClaimName => $"{Action}.{Resource}";
};