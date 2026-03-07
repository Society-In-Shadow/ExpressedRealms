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
                Policies.ManagePowers.Name,
                policy => policy.RequireRole(UserRoles.PowerManagementRole)
            );

            options.AddPolicy(
                Policies.ManageKnowledges.Name,
                policy => policy.RequireRole(UserRoles.KnowledgeManagementRole)
            );

            options.AddPolicy(
                Policies.DownloadCMSReports.Name,
                policy => policy.RequireRole(UserRoles.DownloadCMSReports)
            );

            options.AddPolicy(
                Policies.DownloadExpressionBooklet.Name,
                policy => policy.RequireRole(UserRoles.DownloadExpressionBooklet)
            );

            options.AddPolicy(
                Policies.ManageBlessings.Name,
                policy => policy.RequireRole(UserRoles.ManageBlessingsRole)
            );

            options.AddPolicy(
                Policies.ManageProgressionPaths.Name,
                policy => policy.RequireRole(UserRoles.ManageProgressionPaths)
            );

            options.AddPolicy(
                Policies.ManageModifiers.Name,
                policy => policy.RequireRole(UserRoles.ManageModifiers)
            );

            options.AddPolicy(
                Policies.ManagePlayerExperience.Name,
                policy => policy.RequireRole(UserRoles.ManagePlayerExperience)
            );
        });
    }
}
