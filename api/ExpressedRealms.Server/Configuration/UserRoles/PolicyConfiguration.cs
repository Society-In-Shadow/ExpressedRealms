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
                Policies.ManageKnowledges.Name,
                policy => policy.RequireRole(UserRoles.KnowledgeManagementRole)
            );

            options.AddPolicy(
                Policies.ManageBlessings.Name,
                policy => policy.RequireRole(UserRoles.ManageBlessingsRole)
            );

            options.AddPolicy(
                Policies.ManageModifiers.Name,
                policy => policy.RequireRole(UserRoles.ManageModifiers)
            );
        });
    }
}
