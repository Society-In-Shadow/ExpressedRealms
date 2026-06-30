using System.CommandLine;
using ExpressedRealms.Scaffolder.Generator.Records;

namespace ExpressedRealms.Scaffolder.SubCommands;

internal class CommonArguments
{
    private readonly Argument<string> _entityName = new("singular-name")
    {
        Description = "The singular name of the entity",
    };

    private readonly Argument<string> _entitiesName = new("plural-name")
    {
        Description = "The plural name of the entity",
    };

    private readonly Option<string> _outputOption = new("--out")
    {
        Aliases = { "-o" },
        Description = "Target Directory.  Use . to indicate current directory (Default)",
    };

    private readonly Option<string> _additionalPropertiesArg = new("--properties")
    {
        Aliases = { "-props" },
        Description = """
            Additional properties to generate.

            Format:
              name:type[:required][:min.X][:max.Y][:optional],

            Where:
              "," - Separates multiple properties.
              ":" - Separates the property options.
              "." - Denotes a sub-property option.

            Single Examples:
              Name:string:required
              Age:int:min.1:max.120
              IsActive:bool
            
            Additional Details
              optional - Will treat the property as nullable by adding ?, eg int?
            
            Multiple Examples:
              Name:string:required,Age:int:min.1:max.120,IsActive:bool
            """,
    };

    public Command CreateCommandWithCommonArguments(string commandName)
    {
        Command subCommand = new(commandName);

        subCommand.Arguments.Add(_entityName);
        subCommand.Arguments.Add(_entitiesName);

        subCommand.Options.Add(_outputOption);
        subCommand.Options.Add(_additionalPropertiesArg);

        return subCommand;
    }

    public GenerateUseCasesModel GenerateCommonUseCaseModel(ParseResult parseResult)
    {
        return new GenerateUseCasesModel
        {
            Plural = parseResult.GetRequiredValue(_entitiesName)!,
            Singular = parseResult.GetRequiredValue(_entityName)!,
            AdditionalProperties = GetAdditionalProperties(
                parseResult.GetValue(_additionalPropertiesArg)
            ),
            TargetFolder = parseResult.GetValue(_outputOption)!,
        };
    }

    private static List<PropertyDefinition> GetAdditionalProperties(
        string? additionalPropertiesArgValue
    )
    {
        var additionalProperties = new List<PropertyDefinition>();

        if (string.IsNullOrEmpty(additionalPropertiesArgValue))
            return additionalProperties;

        var propertyList = additionalPropertiesArgValue.Split(",");

        return propertyList
            .Select(x =>
            {
                var parts = x.Split(':');

                var containsMinValue = parts.FirstOrDefault(x => x.StartsWith("min."));
                var containsMaxValue = parts.FirstOrDefault(x => x.StartsWith("max."));

                return new PropertyDefinition(
                    Name: parts[0],
                    Type: parts[1],
                    Required: parts.Length > 2
                        && parts[2].Equals("required", StringComparison.OrdinalIgnoreCase),
                    MinValue: containsMinValue != null
                        ? int.Parse(containsMinValue.Substring(4))
                        : null,
                    MaxValue: containsMaxValue != null
                        ? int.Parse(containsMaxValue.Substring(4))
                        : null,
                    Optional: parts.Any(y => y.Equals("optional", StringComparison.OrdinalIgnoreCase))
                );
            })
            .ToList();
    }
}
