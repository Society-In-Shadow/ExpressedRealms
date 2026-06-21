using System.Text;
using Bogus;
using ExpressedRealms.Scaffolder.Generator.Records;
using Scriban.Runtime;

namespace ExpressedRealms.Scaffolder.Generator.Renderers;

internal static class RenderProperties
{
    internal static void AddRenderClassProperties(this ScriptObject scriptObject)
    {
        scriptObject.Import("render_class_properties", new Func<IEnumerable<PropertyDefinition>, string>(properties =>
        {
            var sb = new StringBuilder();

            foreach (var property in properties)
            {
                sb.AppendLine(
                    $"public {(property.Required ? "required " : "")}{property.Type} {property.Name} {{ get; set; }}"
                );
            }

            return sb.ToString();
        }));
    }
    
    internal static void AddRenderListProperties(this ScriptObject scriptObject)
    {
        scriptObject.Import("render_list_properties", new Func<IEnumerable<PropertyDefinition>, string, string>((properties, accessorName) =>
        {
            var sb = new StringBuilder();

            foreach (var property in properties)
            {
                sb.AppendLine(
                    $"{property.Name} = {accessorName}.{property.Name},"
                );
            }

            return sb.ToString();
        }));
    }
    
    internal static void AddRenderUnitTestAssignmentProperties(this ScriptObject scriptObject)
    {
        Randomizer.Seed = new Random(8675309);
        var faker = new Faker();
        
        scriptObject.Import("render_unit_test_assignment_properties", new Func<IEnumerable<PropertyDefinition>, string, string>((properties, accessorName) =>
        {
            var sb = new StringBuilder();

            foreach (var property in properties)
            {
                var dummyValue = GetDummyValue(property, faker);
                sb.AppendLine(
                    $"{property.Name} = {dummyValue},"
                );
            }

            return sb.ToString();
        }));
    }
    
    private static string GetDummyValue(PropertyDefinition property, Faker faker)
    {
        return property.Type.ToLowerInvariant() switch
        {
            "string" => $"\"{faker.Random.Words(1)}\"",
            "int" => faker.Random.Number().ToString(),
            "bool" => "true",
            "guid" => "Guid.NewGuid()",
            _ => throw new NotSupportedException(property.Type)
        };
    }
    
    internal static void AddRenderAssignmentProperties(this ScriptObject scriptObject)
    {
        scriptObject.Import("render_assignment_properties", new Func<IEnumerable<PropertyDefinition>, string, string, string>((properties, assignerName, accessorName) =>
        {
            var sb = new StringBuilder();

            foreach (var property in properties)
            {
                sb.AppendLine(
                    $"{assignerName}.{property.Name} = {accessorName}.{property.Name};"
                );
            }

            return sb.ToString();
        }));
    }
    
    internal static void AddRenderUnitTestChecksProperties(this ScriptObject scriptObject)
    {
        scriptObject.Import("render_unit_test_check_properties", new Func<List<PropertyDefinition>, string, string, string>((properties, assignerName, accessorName) =>
        {
            var sb = new StringBuilder();

            foreach (var property in properties)
            {
                if (properties.IndexOf(property) == 0)
                {
                    sb.AppendLine(
                        $"{assignerName}.{property.Name} == {accessorName}.{property.Name}"
                    );
                }
                else
                {
                    sb.AppendLine(
                        $"&& {assignerName}.{property.Name} == {accessorName}.{property.Name}"
                    );
                }
            }

            return sb.ToString();
        }));
    }

    internal static void AddRenderUnitTestChecksSingleLineProperties(this ScriptObject scriptObject)
    {
        scriptObject.Import("render_unit_test_check_single_line_properties", new Func<List<PropertyDefinition>, string, string>((properties, singular) =>
        {
            var sb = new StringBuilder();

            foreach (var property in properties)
            {
                sb.AppendLine($"Assert.Equal({singular}DbModel.{property.Name}, results.Value.{property.Name});");
            }

            return sb.ToString();
        }));
    }
    
    internal static void AddRenderUnitTestListObjectProperties(this ScriptObject scriptObject)
    {

        scriptObject.Import("render_unit_test_list_object_properties", new Func<List<PropertyDefinition>, string>((properties) =>
        {
            Randomizer.Seed = new Random(434724);
            var faker = new Faker();
            var sb = new StringBuilder();

            sb.AppendLine($"new ()");
            sb.AppendLine("{");
            sb.AppendLine("    Id = 1,");
            foreach (var property in properties)
            {
                var dummyValue = GetDummyValue(property, faker);
                sb.AppendLine(
                    $"    {property.Name} = {dummyValue},"
                );
            }
            sb.AppendLine("},");
            sb.AppendLine($"new ()");
            sb.AppendLine("{");
            sb.AppendLine("    Id = 2,");
            foreach (var property in properties)
            {
                var dummyValue = GetDummyValue(property, faker);
                sb.AppendLine(
                    $"    {property.Name} = {dummyValue},"
                );
            }
            sb.AppendLine("}");

            return sb.ToString();
        }));
    }
}