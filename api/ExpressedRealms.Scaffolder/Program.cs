using System.CommandLine;
using System.CommandLine.Parsing;
using ExpressedRealms.Scaffolder;

namespace scl;

class Program
{
    static int Main(string[] args)
    {
        Option<string> entityName = new("--singular")
        {
            Aliases = { "-s" },
            Description = "The singular name of the entity",
            Required = true
        };
        Option<string> entitiesName = new("--plural")
        {
            Aliases = { "-p" },
            Description = "The plural name of the entity",
            Required = true
        };
        Option<string> outputOption = new("--out")
        {
            Aliases = { "-o" },
            Description = "Target Directory.  Use . to indicate current directory (Default)"
        };
        Option<string> additionalProperties = new("--properties")
        {
            Aliases = { "-props" },
            Description = "Add additional properties that need to be handled.  Format : \"name:type[:required], etc\""
        };

        RootCommand rootCommand = new("Sample app for System.CommandLine");
        rootCommand.Options.Add(entityName);
        rootCommand.Options.Add(entitiesName);
        rootCommand.Options.Add(outputOption);
        rootCommand.Options.Add(additionalProperties);

        ParseResult parseResult = rootCommand.Parse(args);
        
        var singularArg = parseResult.GetValue(entityName);
        var pluralArg = parseResult.GetValue(entitiesName);
        var outputArg = parseResult.GetValue(outputOption);
        var additionalPropertiesArg = parseResult.GetValue(additionalProperties);
        
        if (parseResult.Errors.Count == 0)
        {
            CrudGenerator.GenerateAPIs( new CrudGenerator.GenerateUseCasesModel(singularArg, pluralArg, "foo", outputArg, additionalPropertiesArg));
            return 0;
        }
        foreach (ParseError parseError in parseResult.Errors)
        {
            Console.Error.WriteLine(parseError.Message);
        }
        return 1;
    }
}
