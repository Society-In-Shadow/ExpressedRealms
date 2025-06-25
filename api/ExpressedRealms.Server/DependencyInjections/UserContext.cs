using ExpressedRealms.Authentication;
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
}
