using System.Security.Claims;
using ExpressedRealms.Admin.Repository;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.Roles;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace ExpressedRealms.Admin.API.CustomClaimsPrincipleFactory;

public class CustomClaimsPrincipalFactory(
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    IOptions<IdentityOptions> options,
    IRolesRepository rolesRepository)
    : UserClaimsPrincipalFactory<User, Role>(userManager, roleManager, options)
{
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
    {
        var identity = await base.GenerateClaimsAsync(user);
        
        var permissions = await rolesRepository.GetPermissionKeysForUserAsync(user.Id);
        var roles = await UserManager.GetRolesAsync(user);
        
        identity.AddClaims(roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList());
        identity.AddClaims(permissions.Select(x => new Claim("custom_permission", x)).ToList());

        return identity;
    }
}
