using ExpressedRealms.Authentication;
using ExpressedRealms.Authentication.PermissionCollection.Support;

namespace ExpressedRealms.Repositories.Shared.ExternalDependencies;

public interface IUserContext
{
    public string CurrentUserId();
    public Task<bool> CurrentUserHasPolicy(Policies policy);
    Task<bool> CurrentUserHasPermission(Permission permission);
}
