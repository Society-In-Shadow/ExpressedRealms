using ExpressedRealms.Authentication.PermissionCollection.Support;

namespace ExpressedRealms.Repositories.Shared.ExternalDependencies;

public interface IUserContext
{
    public string CurrentUserId();
    public bool CurrentUserHasPermission(Permission permission);
}
