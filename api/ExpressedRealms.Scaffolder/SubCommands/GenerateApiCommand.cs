using System.CommandLine;
using ExpressedRealms.Scaffolder.Generator;

namespace ExpressedRealms.Scaffolder.SubCommands;

internal static class GenerateApiCommand
{
    public static void AddApiCommand(this RootCommand rootCommand)
    {
        var commonBase = new CommonArguments();
        var useCaseCommand = commonBase.CreateCommandWithCommonArguments("api");
        useCaseCommand.Aliases.Add("apis");
        
        useCaseCommand.SetAction(parseResult =>
        {
            var model = commonBase.GenerateCommonUseCaseModel(parseResult);
            CrudGenerator.GenerateAPIs(model);
        });
        
        rootCommand.Subcommands.Add(useCaseCommand);
    }
}