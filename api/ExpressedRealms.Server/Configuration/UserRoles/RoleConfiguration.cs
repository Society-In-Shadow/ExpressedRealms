using ExpressedRealms.DB.UserProfile.PlayerDBModels.Roles;
using Microsoft.AspNetCore.Identity;

namespace ExpressedRealms.Server.Configuration.UserRoles;

public static class RoleConfiguration
{
    public static void ConfigureUserRoles(this WebApplication app)
    {
        app.Lifetime.ApplicationStarted.Register(async () =>
        {
            using var scope = app.Services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
            var roles = new[]
            {
                UserRoles.ExpressionEditor,
                UserRoles.UserManagementRole,
                UserRoles.PowerManagementRole,
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(
                        new Role() { Id = Guid.NewGuid().ToString(), Name = role }
                    );
                }
            }
        });
    }
}
