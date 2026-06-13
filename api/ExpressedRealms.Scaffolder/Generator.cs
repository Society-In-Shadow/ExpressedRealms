using Scriban;

public static class CrudGenerator
{
    public record GenerateUseCasesModel(string singular, string plural, string route, string targetFolder);

    public static void GenerateUseCases(GenerateUseCasesModel model)
    {
        var templateRoot =
            Path.Combine(AppContext.BaseDirectory, "Templates", "EntityUseCases");
        
        Generate(model, templateRoot, $"{model.singular}UseCases");
    }

    public static void GenerateAPIs(GenerateUseCasesModel model)
    {
        var templateRoot =
            Path.Combine(AppContext.BaseDirectory, "Templates", "EntityEndpoints");
        
        Generate(model, templateRoot, $"{model.singular}Endpoints");
    }
    
    public static void GenerateRepository(GenerateUseCasesModel model)
    {
        var templateRoot =
            Path.Combine(AppContext.BaseDirectory, "Templates", "EntityRepository");
        
        Generate(model, templateRoot, $"{model.plural}");
    }
    
    
    
    private static void Generate(GenerateUseCasesModel generationModel, string templateRoot, string baseFolderName)
    {
        var outputSource = string.IsNullOrWhiteSpace(generationModel.targetFolder) ? Environment.CurrentDirectory : Path.GetFullPath(generationModel.targetFolder);
        
        var outputRoot = Path.Combine(outputSource, baseFolderName);
        
        var start = outputSource.IndexOf("ExpressedRealms.");

        if (start < 0)
            throw new InvalidOperationException("Base namespace not found.");

        var namespacebase = outputSource.Substring(start);
        
        var parts = namespacebase.Split('.');
        var projectname = parts.Length > 1 ? parts[1] : "";
        
        Console.WriteLine(namespacebase);
        Console.WriteLine(projectname);
        var model = new
        {
            generationModel.singular,
            generationModel.plural,
            generationModel.route,
            namespacebase,
            projectname
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