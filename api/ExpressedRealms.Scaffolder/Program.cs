using System.CommandLine;
using ExpressedRealms.Scaffolder.SubCommands;

RootCommand rootCommand = new("Scaffolder is a command line tool that will generate basic crud api from the api layer all the way down to the repository, including unit tests for the use cases.  You can pass through additional properties that need to be handled.  Format : \"name:type[:required]:[min.20]:[max.15],name:type etc\"");
rootCommand.AddUseCasesCommand();
rootCommand.AddApiCommand();
rootCommand.AddRepositoryCommand();
rootCommand.AddUnitTestsCommand();

return await rootCommand.Parse(args).InvokeAsync();