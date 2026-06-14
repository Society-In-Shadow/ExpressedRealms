using System.CommandLine;
using System.CommandLine.Parsing;
using ExpressedRealms.Scaffolder.Generator;
using ExpressedRealms.Scaffolder.Generator.Records;

namespace scl;

class Program
{
    static int Main(string[] args)
    {
        Argument<string> generationType = new("generation-type")
        {
            Description = "The generation type is the type of files you want to create.  You have Everything, API, UseCase, Tests, Repository. (Case Insensitive)",
        };
        Argument<string> entityName = new("singular-name")
        {
            Description = "The singular name of the entity",
        };
        Argument<string> entitiesName = new("plural-name")
        {
            Description = "The plural name of the entity",
        };
        Option<string> outputOption = new("--out")
        {
            Aliases = { "-o" },
            Description = "Target Directory.  Use . to indicate current directory (Default)"
        };
        Option<string> additionalPropertiesArg = new("--properties")
        {
            Aliases = { "-props" },
            Description = "Add additional properties that need to be handled.  Format : \"name:type[:required], etc\""
        };

        RootCommand rootCommand = new("Sample app for System.CommandLine");
        rootCommand.Arguments.Add(generationType);
        rootCommand.Arguments.Add(entityName);
        rootCommand.Arguments.Add(entitiesName);
        rootCommand.Options.Add(outputOption);
        rootCommand.Options.Add(additionalPropertiesArg);

        ParseResult parseResult = rootCommand.Parse(args);

        if (parseResult.Errors.Count == 0)
        {
            var model = new GenerateUseCasesModel()
            {
                Plural = parseResult.GetRequiredValue(entitiesName)!,
                Singular = parseResult.GetRequiredValue(entityName)!,
                AdditionalProperties = GetAdditionalProperties(parseResult.GetValue(additionalPropertiesArg)),
                TargetFolder = parseResult.GetValue(outputOption)!
            };

            switch (parseResult.GetRequiredValue(generationType).ToLower())
            {
                case "everything":
                    Console.WriteLine("To Be Implemented");
                    break;
                case "api":
                    CrudGenerator.GenerateAPIs(model);
                    break;
                case "usecase":
                case "usecases":
                    CrudGenerator.GenerateUseCases(model);
                    break;
                case "test":
                    CrudGenerator.GenerateUseCaseTests(model);
                    break;
                case "repository":
                    CrudGenerator.GenerateRepository(model);
                    break;
            }
            return 0;
        }
        foreach (ParseError parseError in parseResult.Errors)
        {
            Console.Error.WriteLine(parseError.Message);
        }
        return 1;
    }

    private static List<PropertyDefinition> GetAdditionalProperties(string? additionalPropertiesArgValue)
    {
        var additionalProperties = new List<PropertyDefinition>();
        
        if(string.IsNullOrEmpty(additionalPropertiesArgValue))
            return additionalProperties;
        
        var propertyList = additionalPropertiesArgValue.Split(",");

        return propertyList.Select(x =>
        {
            var parts = x.Split(':');

            return new PropertyDefinition(
                Name: parts[0],
                Type: parts[1],
                Required: parts.Length > 2 &&
                          parts[2].Equals("required",
                              StringComparison.OrdinalIgnoreCase));
        }).ToList();
    }
}
