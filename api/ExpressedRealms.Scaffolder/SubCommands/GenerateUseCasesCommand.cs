using System.CommandLine;
using ExpressedRealms.Scaffolder.Generator;

namespace ExpressedRealms.Scaffolder.SubCommands;

internal static class GenerateUseCasesCommand
{
    public static void AddUseCasesCommand(this RootCommand rootCommand)
    {
        var commonBase = new CommonArguments();
        var useCaseCommand = commonBase.CreateCommandWithCommonArguments("usecases");
        useCaseCommand.Aliases.Add("usecase");
        
        useCaseCommand.SetAction(parseResult =>
        {
            var model = commonBase.GenerateCommonUseCaseModel(parseResult);
            CrudGenerator.GenerateUseCases(model);
        });
        
        rootCommand.Subcommands.Add(useCaseCommand);
    }
}