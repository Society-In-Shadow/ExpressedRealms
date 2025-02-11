using Microsoft.AspNetCore.Identity;

namespace ExpressedRealms.Server.Configuration.UserRoles;

public static class RoleConfiguration
{
    public static void ConfigureUserRoles(this WebApplication app)
    {
        app.Lifetime.ApplicationStarted.Register(async () =>
        {
            using var scope = app.Services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roles = new[] { UserRoles.ExpressionEditor, UserRoles.UserManagementRole };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        });
    }
}
