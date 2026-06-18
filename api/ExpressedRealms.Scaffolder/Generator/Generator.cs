using ExpressedRealms.Scaffolder.Generator.Records;
using ExpressedRealms.Scaffolder.Generator.Renderers;
using Scriban;
using Scriban.Runtime;

namespace ExpressedRealms.Scaffolder.Generator;

public static class CrudGenerator
{
    public static void GenerateUseCases(GenerateUseCasesModel model)
    {
        var templateRoot =
            Path.Combine(AppContext.BaseDirectory, "Templates", "EntityUseCases");
        
        Generate(model, templateRoot, $"{model.Singular}UseCases");
    }

    public static void GenerateAPIs(GenerateUseCasesModel model)
    {
        var templateRoot =
            Path.Combine(AppContext.BaseDirectory, "Templates", "EntityEndpoints");
        
        Generate(model, templateRoot, $"{model.Singular}Endpoints");
    }
    
    public static void GenerateRepository(GenerateUseCasesModel model)
    {
        var templateRoot =
            Path.Combine(AppContext.BaseDirectory, "Templates", "EntityRepository");
        
        Generate(model, templateRoot, $"{model.Plural}");
    }
    
    
    public static void GenerateUseCaseTests(GenerateUseCasesModel model)
    {
        var templateRoot =
            Path.Combine(AppContext.BaseDirectory, "Templates", "EntitiesUseCaseTests");
        
        Generate(model, templateRoot, $"{model.Plural}");
    }
    
    
    private static void Generate(GenerateUseCasesModel generationModel, string templateRoot, string baseFolderName)
    {
        var outputSource = string.IsNullOrWhiteSpace(generationModel.TargetFolder) ? Environment.CurrentDirectory : Path.GetFullPath(generationModel.TargetFolder);
        
        var outputRoot = Path.Combine(outputSource, baseFolderName);
        
        var start = outputSource.IndexOf("ExpressedRealms.");

        if (start < 0)
            throw new InvalidOperationException("Base namespace not found.");

        var namespacebase = outputSource.Substring(start);
        
        var parts = namespacebase.Split('.');
        var projectname = parts.Length > 1 ? parts[1] : "";
        
        var model = new
        {
            singular = generationModel.Singular,
            plural = generationModel.Plural,
            namespacebase,
            projectname,
            properties = generationModel.AdditionalProperties
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
            var context = new TemplateContext();

            var scriptObject = new ScriptObject();
            scriptObject.Import(model);
            scriptObject.AddRenderClassProperties();
            scriptObject.AddRenderListProperties();
            scriptObject.AddRenderAssignmentProperties();
            scriptObject.AddValidationPropertyTests();
            scriptObject.AddRenderUnitTestChecksProperties();
            scriptObject.AddRenderUnitTestAssignmentProperties();

            context.PushGlobal(scriptObject);

            var result = template.Render(context);

            File.WriteAllText(
                Path.Combine(outputDir, fileName),
                result);

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