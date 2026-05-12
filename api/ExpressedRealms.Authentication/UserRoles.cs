namespace ExpressedRealms.Server.Configuration.UserRoles;

public static class UserRoles
{
    public const string ManageModifiers = "ManageModifiers";

    /// <summary>
    /// Add items to this list to automatically add them to the database.
    /// </summary>
    public static string[] RolesForPermissions => new[] { ManageModifiers };
}
