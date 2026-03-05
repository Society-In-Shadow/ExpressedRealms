using ExpressedRealms.Authentication;
using ExpressedRealms.Authentication.PermissionCollection.Support;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using ExpressedRealms.Server.Shared;
using ExpressedRealms.Server.Shared.Extensions;

namespace ExpressedRealms.Server.DependencyInjections;

public class UserContext(IHttpContextAccessor accessor) : IUserContext
{
    public string CurrentUserId()
    {
        return accessor.HttpContext.User.GetUserId();
    }

    public async Task<bool> CurrentUserHasPolicy(Policies policy)
    {
        return await accessor.HttpContext.UserHasPolicyAsync(policy);
    }

    public async Task<bool> CurrentUserHasPermission(Permission permission)
    {
        return accessor.HttpContext.User.UserHasPermission(permission);
    }
}
