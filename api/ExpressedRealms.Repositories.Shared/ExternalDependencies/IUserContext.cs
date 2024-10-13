using ExpressedRealms.Authentication;

namespace ExpressedRealms.Repositories.Shared.ExternalDependencies;

public interface IUserContext
{
    public string CurrentUserId();
    public Task<bool> CurrentUserHasPolicy(Policies policy);
}
