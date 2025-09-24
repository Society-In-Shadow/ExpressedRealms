namespace ExpressedRealms.Server.Configuration.UserRoles;

public static class UserRoles
{
    public const string ExpressionEditor = "ExpressionEditorRole";
    public const string UserManagementRole = "UserManagementRole";
    public const string PowerManagementRole = "PowerManagementRole";
    public const string KnowledgeManagementRole = "KnowledgeManagementRole";
    public const string DownloadCMSReports = "DownloadCMSReports";
    public const string DownloadExpressionBooklet = "DownloadExpressionBooklet";
    public const string ManageBlessingsRole = "ManageBlessingsRole";
    public const string ManagePlayerCharacterList = "ManagePlayerCharacterList";

    /// <summary>
    /// Add items to this list to automatically add them to the database.
    /// </summary>
    public static string[] RolesForPermissions =>
        new[]
        {
            ExpressionEditor,
            UserManagementRole,
            PowerManagementRole,
            KnowledgeManagementRole,
            DownloadCMSReports,
            DownloadExpressionBooklet,
            ManageBlessingsRole,
            ManagePlayerCharacterList,
        };
}
