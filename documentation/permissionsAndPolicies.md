# Permissions and Policies

## RBAC System
Typically referred to as Role Based Access Control.  In our case it's a Resource Based Access Control system.
Each resource has a set of permissions associated with it, eg, a user can add, edit, view, delete, and publish events.

### Roles
Roles are fully defined by the admins of the system in the app, it's up to them to define what permissions each role has. Correspondingly,
it's up to them to know that in order to modify schedule event items, they need to have the read permission for Events.

### Resource
A resource is essentially a domain object, a thing you can do something with in the UI.

### Permissions
A permission is effectively an action you can take on a resource, eg add, edit, delete, view, etc.

It's typically a 1:1 with the endpoints related to a resource.

## Defining Permissions
Permissions are flexible to an extent, they inherit from an IPermission interface in the Authentication project.
There is a function using reflection that will grab the IPermission objects, and create a list of resources and actions.

That can be translated into permissions.

For our app, the structure will be Permissions.Resource.Action to get to the permission.

### Using Permissions
There's a RequirePermission extension method on the endpoint builder that you can use to enforce permissions.

Below is an example of using it, along with the older policy based approach.

As the migration is going, this functionality will be hidden behind a feature flag. That needs to be
enabled in order for the permissions to work.

```csharp
endpointGroup
   .MapPost("", CreateEventEndpoint.ExecuteAsync)
   .RequirePolicyAuthorization(Policies.ManageEvents)
   .RequirePermission(Permissions.Event.Create);
```

### Permission Syncing
The C# objects that inherit from IPermission are the source of truth for the system.  On app startup, it will grab all
permissions code side and compare them to what's in the database.  If there are differences, it will update accordingly.

#### Database Update
- On creation / update - it will simply add / update the corresponding record
- On Deletion / Removal
  - It will remove it from any policies
  - It will remove the action(s)
  - If there are no actions, it will remove the resource.

#### UI / TS Update

There is a helper project (PermissionSync), that is a console app that will output TS file structured string of permissions,
similar to the example below.  

This is meant to automate adding these to the front end, as manually doing it is tedious.

You do need to run the rebuild script to get the latest version of the permissions, ideally before
committing any changes to the code base.

```ts
/**
 * Auto-Generated, Do Not Edit
 */
export const Permissions = {
  Event: {
     Edit: 'event.edit',
     View: 'event.view',
     Create: 'event.create',
     Delete: 'event.delete',
     Publish: 'event.publish',
  } as const,
  EventScheduleItem: {
     Edit: 'eventscheduleitem.edit',
     View: 'eventscheduleitem.view',
     Create: 'eventscheduleitem.create',
     Delete: 'eventscheduleitem.delete',
  } as const,
} as const

export type Permission = typeof Permissions[keyof typeof Permissions]
```

The rebuild bash script is setup to grab that output, generate [Permission.ts file](./../client/src/types/Permissions.ts)
and put in the appropriate place.  

## Legacy Permission / Policy System
Keeping this around till full conversion has happened.

### Adding Permissions / Policies
As of right now, a Permission = Policy, it's a one to one relationship.  Mainly, because this will eventually be a fully
fledged RBAC system, a system where users can define their own roles and assign those out.  For now, we are waiting for
there to be enough permissions where handling those become a hassle.

### Testing Permissions on Use Case Level

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

### Handling Unit Tests on Use Case Level

This is actually straight forward, just use the following to mock the policy.
```csharp
A.CallTo(() => userContext.CurrentUserHasPolicy(Policies.ManageEvents)).Returns(true);
```
