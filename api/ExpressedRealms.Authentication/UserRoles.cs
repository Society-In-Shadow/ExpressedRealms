namespace ExpressedRealms.Server.Configuration.UserRoles;

public static class UserRoles
{
    public const string ExpressionEditor = "ExpressionEditorRole";
    public const string PowerManagementRole = "PowerManagementRole";
    public const string KnowledgeManagementRole = "KnowledgeManagementRole";
    public const string ManageBlessingsRole = "ManageBlessingsRole";
    public const string ManageProgressionPaths = "ManageProgressionPaths";
    public const string ManageModifiers = "ManageModifiers";
    public const string ManagePlayerExperience = "ManageUserExperience";

    /// <summary>
    /// Add items to this list to automatically add them to the database.
    /// </summary>
    public static string[] RolesForPermissions =>
        new[]
        {
            ExpressionEditor,
            PowerManagementRole,
            KnowledgeManagementRole,
            ManageBlessingsRole,
            ManageProgressionPaths,
            ManageModifiers,
            ManagePlayerExperience,
        };
}
