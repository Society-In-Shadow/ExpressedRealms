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
        });
    }
}
