# Repository Standards

## Repository
The repository itself should be an internal sealed class, that has a public interface.  There should be a static
dependency class that connects them that external callers can call.  All method calls will be suffixed with "Async" if 
appropriate.

## Using Results Return Type

### Handling Error Types
All failure types will have an associated class to deal pass back.  It will allow the API layer to specifically deal
any specific errors.  They will:

They Will:
* Always end in "Failure"
* Return a message or something useful to the caller

Here is a basic structure:

```csharp
public sealed class AlreadyDeletedFailure: Error
{
    public AlreadyDeletedFailure(string objectName)
    {
        Message = $"{objectName} was already deleted.";
    }
}
```

You can also have state management ones, where you want the caller to handle the data in a specific way, such as:

```csharp
public sealed class FluentValidationFailure : Error
{
    public IDictionary<string, string[]> ValidationFailures;
    public FluentValidationFailure(IDictionary<string, string[]> validationFailures)
    {
        ValidationFailures = validationFailures;
    }
}
```

## Enum Use
* Each enum will be in a separate class
* Enums will all be located in the same folder

## Data Transfer Objects (Dto)
* Classes will have "Dto" at the end of the class name
* Validation Classes will have "DtoValidator" at the end of the class name
* The DTO's should always be public sealed records
* The Validation Classes will have internal sealed class if the incoming data needs to be validated
* Repository Methods will immediately call for the Fluent Validation
* Fluent Validation can have async operators to check for Ids dynamically

## Common Library
### Common Failure Types
There are three Result Failure Types
* Already Deleted Failure - Indicates that the resource has already been deleted
* Fluent Validation Failure - Returns a diction of any fluent validation errors
* Not Found Failure - Indicates that the record was not found****

### IUserContext
This is an interface that needs to be full-filled by the caller of the repositories.  It mainly passes through the user
id from the front end.  The server project should have this mapped out in Dependency Injections folder

