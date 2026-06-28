using System.Reflection;
using System.Text;
using ExpressedRealms.FeatureFlags;

var resources = ReleaseFlags.List;

var fieldNames = typeof(ReleaseFlags)
    .GetFields(BindingFlags.Public | BindingFlags.Static)
    .Where(f => f.FieldType == typeof(ReleaseFlags))
    .ToDictionary(f => (ReleaseFlags)f.GetValue(null)!, f => f.Name);

var builder = new StringBuilder();

builder.AppendLine("/**");
builder.AppendLine(" * Auto-Generated, Do Not Edit");
builder.AppendLine(" */");

builder.AppendLine("export const FeatureFlags = {");
foreach (var resource in resources)
{
    builder.AppendLine($"  {fieldNames[resource]}: \'{resource.Value}\',");
}

builder.AppendLine("} as const");
builder.AppendLine();
builder.Append("export type FeatureFlag = (typeof FeatureFlags)[keyof typeof FeatureFlags]");

Console.WriteLine(builder.ToString());
