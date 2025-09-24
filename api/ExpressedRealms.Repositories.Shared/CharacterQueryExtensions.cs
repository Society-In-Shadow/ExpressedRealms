using ExpressedRealms.Authentication;
using ExpressedRealms.DB.Characters;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;

namespace ExpressedRealms.Repositories.Shared;

public static class CharacterQueryExtensions
{
    public static async Task<IQueryable<Character>> WithUserAccessAsync(
        this IQueryable<Character> query,
        IUserContext userContext,
        int characterId
    )
    {
        if (await userContext.CurrentUserHasPolicy(Policies.ManagePlayerCharacterList))
        {
            return query.Where(x =>
                x.Id == characterId
                && (x.IsPrimaryCharacter || x.Player.UserId == userContext.CurrentUserId())
            );
        }
        return query.Where(x =>
            x.Id == characterId && x.Player.UserId == userContext.CurrentUserId()
        );
    }
}
