using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace ExpressedRealms.Admin.API.AdminEndpoints.BypassEmailConfirmation;

public static class BypassEmailConfirmationEndpoint
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

        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        await userManager.ConfirmEmailAsync(user, token);

        return TypedResults.NoContent();
    }
}
