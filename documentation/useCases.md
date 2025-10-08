# Use Cases

These are where most of the business logic is located, and tested against.  All API input validations are done through
this as well.

## Rider
I have gone through and created templates in Rider to allow the quick addition of use cases.
It's Right Click > Add > Use Case, there you can pick the type of file, as listed below.

You might need to import team settings in order for that to work

## Basic Structure
All components of a use case will be self contained in a dedicated folder.

The folder will have most if not all of the following files in it:
 - Model
 - ModelValidator
 - UseCase
 - IUseCase
 - ReturnModel

### Model
This is the properties that the use case needs to run.  This should alwasy be a public class

### Model Validator
This uses Fluent Validations, and will typically house logic for ensuring that id's exist, ensuring that data is formatted
correctly, and various other edge cases.

This will always be an internal sealed class, as we don't need this outside of the current assembly

In the configuration portion of the project, make sure to include the following to automatically inject all validators
for the project

```csharp
services.ImportValidators(Assembly.GetExecutingAssembly());
```

### Use Case
This is the meat and potatoes of the business logic.  All business logic goes into here. If the validator returns errors,
the use case will return the validation errors and exit early.  At which point the API will return the validation results.

This will also be an internal sealed class, as projects should not have direct access to this.

Otherwise, most use cases are typically add / edit / delete or get variants.

### I Use Case
This is the interface for the use case.  It should inherit from "IGenericUseCase<T, M>"
or "IGenericUseCase<T>".  T is the return type (Always some version of Result<t>) or M is the model the use case needs.

This will be a public interface, all other projects will refer to this to grab items

In the configuration section of the project, you should add the following, to automatically inject all use cases.

```csharp
services.ImportGenericUseCases(Assembly.GetExecutingAssembly());
```

### Return Model
In situations where it needs to return data, you should always have a base class that can be easily extended.  This is 
the return model.

The only real exception to this rule is create cases, those typically only return an ID, not whole objects.

## Testing
There is a UseCases.Tests.Unit project that contains infrastructure testing.  These should be included in the corresponding
testing project.

It will make sure the following is true

 - Use Cases end with a file name "UseCase"
 - Use Cases will never call Db Context directly
 - Tests to make sure that unit test are named in a certain way (Starts with ValidationFor or UseCase)
 - Use case has a corresponding Model and validator, with correct names / dependencies
 - Use cases project and assembly tests have the same root name
