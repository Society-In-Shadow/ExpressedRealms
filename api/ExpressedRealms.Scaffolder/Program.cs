using System.CommandLine;
using System.CommandLine.Parsing;

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
            Description = "Target Directory.  Use . to indicate current directory"
        };

        RootCommand rootCommand = new("Sample app for System.CommandLine");
        rootCommand.Options.Add(entityName);
        rootCommand.Options.Add(entitiesName);
        rootCommand.Options.Add(outputOption);

        ParseResult parseResult = rootCommand.Parse(args);
        
        var singularArg = parseResult.GetValue(entityName);
        var pluralArg = parseResult.GetValue(entitiesName);
        var outputArg = parseResult.GetValue(outputOption);
        
        if (parseResult.Errors.Count == 0)
        {
            CrudGenerator.GenerateAPIs( new CrudGenerator.GenerateUseCasesModel(singularArg, pluralArg, "foo", outputArg));
            return 0;
        }
        foreach (ParseError parseError in parseResult.Errors)
        {
            Console.Error.WriteLine(parseError.Message);
        }
        return 1;
    }
}
