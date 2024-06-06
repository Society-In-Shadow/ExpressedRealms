using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using ExpressedRealms.Server.Extensions;

namespace ExpressedRealms.Server.DependencyInjections;

public class UserContext : IUserContext
{
    private readonly HttpContext _httpContext;

    public UserContext(HttpContext httpContext)
    {
        _httpContext = httpContext;
    }

    public string CurrentUserId()
    {
        return _httpContext.User.GetUserId();
    }
}
