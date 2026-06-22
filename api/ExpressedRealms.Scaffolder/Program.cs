using System.CommandLine;
using ExpressedRealms.Scaffolder.SubCommands;

RootCommand rootCommand = new("""
                              This scaffolder is a command line tool that will generate basic crud api from the api layer all the way down to the repository, including unit tests for the use cases.
                              This also includes basic support for defining the properties that need to be passed around and handled on the various layers."
                              """);
rootCommand.AddUseCasesCommand();
rootCommand.AddApiCommand();
rootCommand.AddRepositoryCommand();
rootCommand.AddUnitTestsCommand();

return await rootCommand.Parse(args).InvokeAsync();