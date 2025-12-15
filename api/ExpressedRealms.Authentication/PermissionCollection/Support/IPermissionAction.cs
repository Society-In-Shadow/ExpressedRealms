namespace ExpressedRealms.Authentication.PermissionCollection.Support;

public interface IPermissionAction
{
    string Resource { init; }
    string Action { init; }
    string ClaimName { get; }
}