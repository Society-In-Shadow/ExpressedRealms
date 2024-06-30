# Common Library
## Common Failure Types
There are three Result Failure Types
* Already Deleted Failure - Indicates that the resource has already been deleted
* Fluent Validation Failure - Returns a diction of any fluent validation errors
* Not Found Failure - Indicates that the record was not found****

## IUserContext
This is an interface that needs to be full-filled by the caller of the repositories.  It mainly passes through the user
id from the front end.  The server project should have this mapped out in Dependency Injections folder

