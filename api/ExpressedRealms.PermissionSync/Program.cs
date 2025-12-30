using System.Text;
using ExpressedRealms.Authentication.PermissionCollection.PermissionManager.PermissionHelper;

var resources = PermissionManagerHelper.GetCodeSidePermissions();

var builder = new StringBuilder();

builder.AppendLine("/**");
builder.AppendLine(" * Auto-Generated, Do Not Edit");
builder.AppendLine(" */");

builder.AppendLine("export const Permissions = {");
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
builder.Append("export type Permission = typeof Permissions[keyof typeof Permissions]");

Console.WriteLine(builder.ToString());
