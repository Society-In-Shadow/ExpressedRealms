using Scriban;

public static class CrudGenerator
{
    public static void Generate(string singular, string plural, string route, string output)
    {


        var templateRoot =
            Path.Combine(AppContext.BaseDirectory, "Templates", "EntityEndpoints");

        var outputSource = string.IsNullOrWhiteSpace(output) ? Environment.CurrentDirectory : Path.GetFullPath(output);
        
        var outputRoot =
            Path.Combine(outputSource, $"{singular}Endpoints");
        
        var start = outputSource.IndexOf("ExpressedRealms.");

        if (start < 0)
            throw new InvalidOperationException("Base namespace not found.");

        var namespacebase = outputSource.Substring(start);
        
        Console.WriteLine(namespacebase);
        var model = new
        {
            singular,
            plural,
            route,
            namespacebase
        };
        
        GenerateDirectory(templateRoot, outputRoot, model);
    }

    private static void GenerateDirectory(
        string templateDir,
        string outputDir,
        object model)
    {
        Directory.CreateDirectory(outputDir);

        foreach (var directory in Directory.GetDirectories(templateDir))
        {
            var dirName = Transform(Path.GetFileName(directory), model);

            GenerateDirectory(
                directory,
                Path.Combine(outputDir, dirName),
                model);
        }

        foreach (var file in Directory.GetFiles(templateDir))
        {
            var fileName = Transform(Path.GetFileName(file), model)
                .Replace(".sbn", ".cs");

            var templateText = File.ReadAllText(file);
            var template = Template.Parse(templateText);
            var rendered = template.Render(model);

            File.WriteAllText(
                Path.Combine(outputDir, fileName),
                rendered);

            Console.WriteLine($"Created {fileName}");
        }
    }

    private static string Transform(string input, object model)
    {
        var singular = Get(model, "singular");
        var plural = Get(model, "plural");

        return input
            .Replace("Entity", singular)
            .Replace("Entities", plural);
    }

    private static string Get(object model, string prop)
    {
        return model.GetType()
            .GetProperty(prop)!
            .GetValue(model)!
            .ToString()!;
    }
}