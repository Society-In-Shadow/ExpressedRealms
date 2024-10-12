namespace ExpressedRealms.Server.Configuration.UserRoles;

public static class PolicyConfiguration
{
    public static void AddPolicyConfiguration(
        this WebApplicationBuilder builder
    )
    {
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.ExpressionEditorPolicy, policy =>
                policy.RequireRole(UserRoles.ExpressionEditor));
        });
    }
}