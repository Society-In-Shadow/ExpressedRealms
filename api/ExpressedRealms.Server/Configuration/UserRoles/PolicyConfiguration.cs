using ExpressedRealms.Authentication;

namespace ExpressedRealms.Server.Configuration.UserRoles;

public static class PolicyConfiguration
{
    public static void AddPolicyConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(
                Policies.ExpressionEditorPolicy.Name,
                policy => policy.RequireRole(UserRoles.ExpressionEditor)
            );

            options.AddPolicy(
                Policies.UserManagementPolicy.Name,
                policy => policy.RequireRole(UserRoles.UserManagementRole)
            );

            options.AddPolicy(
                Policies.ManagePowers.Name,
                policy => policy.RequireRole(UserRoles.PowerManagementRole)
            );
        });
    }
}
