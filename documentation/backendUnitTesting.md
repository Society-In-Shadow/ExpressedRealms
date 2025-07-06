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

# Shared Infrastructure Unit Tests for Use Cases
When creating new unit test projects for use cases, you need to make sure you add the following class to the root of
that assembly.

This will ensure that the use case and unit tests are following good practices as defined by the unit tests.

```csharp
[UsedImplicitly]
public class AssemblyInfrastructureTests : InfrastructureTests
{
}
```