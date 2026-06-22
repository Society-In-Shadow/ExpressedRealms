using System.CommandLine;
using ExpressedRealms.Scaffolder.Generator;

namespace ExpressedRealms.Scaffolder.SubCommands;

internal static class GenerateUnitTestsCommand
{
    public static void AddUnitTestsCommand(this RootCommand rootCommand)
    {
        var commonBase = new CommonArguments();
        var useCaseCommand = commonBase.CreateCommandWithCommonArguments("tests");
        useCaseCommand.Aliases.Add("test");
        useCaseCommand.Aliases.Add("unittest");
        useCaseCommand.Aliases.Add("unittests");

        useCaseCommand.Description =
            "Intended to be used in the root of the Unit Test project.  Will create tests for all use cases generate by the use case command.";

        useCaseCommand.SetAction(parseResult =>
        {
            var model = commonBase.GenerateCommonUseCaseModel(parseResult);
            CrudGenerator.GenerateAPIs(model);
        });

        rootCommand.Subcommands.Add(useCaseCommand);
    }
}
