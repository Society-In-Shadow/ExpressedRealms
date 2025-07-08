# Unit Testing the Backend
We use nUnit, FakeItEasy, and NetArchTest.Rules to test the backend logic.

## nUnit
Fairly common library, it's used to do the assertions, and provide the syntax to structure / create the tests themselves.

## FakeItEasy
This is the mocking library we use, we use it to create fakes that we can test against.  Primarily used for method testing
and setting up the dependencies

## NetArchTest.Rules
This is an infrastructure based testing library.  This allows us to enforce common structures and guidelines:
- Any class that inherits from IGenericUseCase needs to have "UseCase" at the end of the class name.
- Use Cases cannot directly call the Db Context
- Method names on unit tests need to follow certain rules and guidelines

## Shared Infrastructure Unit Tests for Use Cases
When creating new unit test projects for use cases, you need to make sure you add the following class to the root of
that assembly.

This will ensure that the use case and unit tests are following good practices as defined by the unit tests.

```csharp
[UsedImplicitly]
public class AssemblyInfrastructureTests : InfrastructureTests
{
}
```

### Note About Not Finding Assembly
If the tests says it cannot find the file to load the assembly, make sure that the unit test project has it as a
referenced project on the nuget / dependency side.

## Creating Custom Assertions
Rider and sonar cloud will not pick up test assertions if they are custom.

You can get around this by adding any of these names to the list:
- ASSERT
- CHECK
- EXPECT
- MUST
- SHOULD
- VERIFY
- VALIDATE

per [Sonar Cloud Documentation](https://community.sonarsource.com/t/how-to-mark-custom-methods-as-assertion-methods-in-c/31437/2)

## Setting Up Projects

### Project to be tested
In the project that is to be tested, you want to add these two lines to the csproj, where the first line targets the test
project.  This allows the test project to access the internals of this project.  This allows the tests to test the
concrete classes that should be internal.

```xml
  <ItemGroup>
    <InternalsVisibleTo Include="ExpressedRealms.Expressions.UseCases.Tests.Unit" />
    <InternalsVisibleTo Include="DynamicProxyGenAssembly2" />
  </ItemGroup>

```

As for the test project, you can add these as dependencies

```xml
  <ItemGroup>
    <PackageReference Include="FakeItEasy" Version="8.3.0" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.14.1" />
    <PackageReference Include="xunit.runner.visualstudio" Version="3.1.1">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
    <PackageReference Include="xunit.v3" Version="2.0.3" />
  </ItemGroup>
```