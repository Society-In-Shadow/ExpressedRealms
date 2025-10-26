# Permissions and Policies

## Adding Permissions / Policies
As of right now, a Permission = Policy, it's a one to one relationship.  Mainly, because this will eventually be a fully
fledged RBAC system, a system where users can define their own roles and assign those out.  For now, we are waiting for
there to be enough permissions where handling those become a hassle.

## Testing Permissions on Use Case Level

When testing on a use case level, you want to import the IUserContext, and use the CurrentUserHasPolicy method like below
This will tie into the overall policy object mentioned above.

```csharp
//                                                                       \/\/\/\/\/\/\/\/\/\/\/\/
internal sealed class GetEventsUseCase(IEventRepository eventRepository, IUserContext userContext) : IGetEventUseCase
{
    public async Task<Result<EventBaseReturnModel>> ExecuteAsync()
    {
        //              \/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/
        var hasPolicy = await userContext.CurrentUserHasPolicy(Policies.ManageEvents);
        // ...
    }
}
```

## Handling Unit Tests on Use Case Level

This is actually straight forward, just use the following to mock the policy.
```csharp
A.CallTo(() => userContext.CurrentUserHasPolicy(Policies.ManageEvents)).Returns(true);
```
