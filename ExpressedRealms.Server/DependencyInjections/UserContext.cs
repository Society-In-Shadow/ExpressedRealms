using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using ExpressedRealms.Server.Extensions;

namespace ExpressedRealms.Server.DependencyInjections;

public class UserContext(IHttpContextAccessor accessor) : IUserContext
{
    public string CurrentUserId()
    {
        return accessor.HttpContext.User.GetUserId();
    }
}
