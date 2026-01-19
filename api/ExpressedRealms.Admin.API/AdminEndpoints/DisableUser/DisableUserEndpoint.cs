using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace ExpressedRealms.Admin.API.AdminEndpoints.DisableUser;

public static class DisableUserEndpoint
{
    public static async Task<Results<NoContent, NotFound>> Execute(
        string userId,
        UserManager<User> userManager
    )
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return TypedResults.NotFound();
        }

        await userManager.SetLockoutEndDateAsync(user, DateTime.MaxValue);

        return TypedResults.NoContent();
    }
}
