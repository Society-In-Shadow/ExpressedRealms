using System.CommandLine;
using ExpressedRealms.Scaffolder.Generator;

namespace ExpressedRealms.Scaffolder.SubCommands;

internal static class GenerateRepositoriesCommand
{
    public static void AddRepositoryCommand(this RootCommand rootCommand)
    {
        var commonBase = new CommonArguments();
        var useCaseCommand = commonBase.CreateCommandWithCommonArguments("repository");
        useCaseCommand.Aliases.Add("repositories");
        useCaseCommand.Description = """
            Intended to be used in the root of the Repository project.  Will create basic repository and interface used by use cases.
            You will need to connect the generated file to DI.
            """;

        useCaseCommand.SetAction(parseResult =>
        {
            var model = commonBase.GenerateCommonUseCaseModel(parseResult);
            CrudGenerator.GenerateRepository(model);
        });

        rootCommand.Subcommands.Add(useCaseCommand);
    }
}
