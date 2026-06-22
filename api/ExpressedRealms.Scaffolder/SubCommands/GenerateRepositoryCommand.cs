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
        
        useCaseCommand.SetAction(parseResult =>
        {
            var model = commonBase.GenerateCommonUseCaseModel(parseResult);
            CrudGenerator.GenerateRepository(model);
        });
        
        rootCommand.Subcommands.Add(useCaseCommand);
    }
}