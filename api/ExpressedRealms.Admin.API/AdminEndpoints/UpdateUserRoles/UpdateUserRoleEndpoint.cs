using ExpressedRealms.DB.UserProfile.PlayerDBModels.Roles;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace ExpressedRealms.Admin.API.AdminEndpoints.UpdateUserRoles;

public static class UpdateUserRoleEndpoint
{
    public static async Task<Results<NoContent, NotFound, BadRequest<string>>> Execute (
        string userId,
        UpdateUserRoleRequest dto,
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        SignInManager<User> signInManager
    )
    {
        var user = await userManager.FindByIdAsync(dto.UserId);

        if (user == null)
        {
            return TypedResults.NotFound();
        }

        // Ensure the role exists before assigning
        if (!await roleManager.RoleExistsAsync(dto.RoleName))
        {
            return TypedResults.NotFound();
        }

        if (dto.IsEnabled)
        {
            var result = await userManager.AddToRoleAsync(user, dto.RoleName);
            if (result.Succeeded)
            {
                return TypedResults.NoContent();
            }
        }
        else
        {
            var result = await userManager.RemoveFromRoleAsync(user, dto.RoleName);
            if (result.Succeeded)
            {
                return TypedResults.NoContent();
            }
        }

        await signInManager.RefreshSignInAsync(user);

        return TypedResults.BadRequest<string>("The role was not updated.");
    }
}
