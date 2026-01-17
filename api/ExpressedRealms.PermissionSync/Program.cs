using System.Text;
using ExpressedRealms.Authentication.PermissionCollection.PermissionManager.PermissionHelper;

var resources = PermissionManagerHelper.GetCodeSidePermissions();

var builder = new StringBuilder();

builder.AppendLine("/**");
builder.AppendLine(" * Auto-Generated, Do Not Edit");
builder.AppendLine(" */");

builder.AppendLine("export const UserPermissions = {");
foreach (var resource in resources)
{
    builder.AppendLine($"  {resource.Name}: {{");

    foreach (var permission in resource.Permissions)
    {
        builder.AppendLine($"    {permission.Name}: \'{permission.Key}\',");
    }
    builder.AppendLine("  } as const,");
}

builder.AppendLine("} as const");
builder.AppendLine();
builder.AppendLine("type NestedValues<T> = T extends Record<string, unknown> ? NestedValues<T[keyof T]> : T");
builder.Append("export type UserPermission = NestedValues<typeof UserPermissions>");

Console.WriteLine(builder.ToString());
