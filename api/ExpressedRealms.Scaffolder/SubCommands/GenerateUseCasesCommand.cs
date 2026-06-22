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
        useCaseCommand.Description =
            "Intended to be used in the root of the Use Case project.  Will create all use cases for the end points.";

        useCaseCommand.SetAction(parseResult =>
        {
            var model = commonBase.GenerateCommonUseCaseModel(parseResult);
            CrudGenerator.GenerateUseCases(model);
        });

        rootCommand.Subcommands.Add(useCaseCommand);
    }
}
