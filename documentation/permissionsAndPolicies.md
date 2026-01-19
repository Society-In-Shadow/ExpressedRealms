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

There is a console app called (PermissionSync), that will output TS file structured string of permissions, per local build.

This is meant to automate adding these to the front end, as manually doing it is tedious.

You do need to run the rebuild script to get the latest version of the permissions, ideally before
committing any changes to the code base.

```ts
/**
 * Auto-Generated, Do Not Edit
 */
export const UserPermissions = {
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

type NestedValues<T> = T extends Record<string, unknown> ? NestedValues<T[keyof T]> : T
export type UserPermission = NestedValues<typeof UserPermissions>
```

The rebuild bash script is setup to grab that output, generate [UserPermissions.ts file](./../client/src/types/UserPermissions.ts)
and put in the appropriate place.

## Using Permissions on the Front End
When using permissions on the front end, you will want to import the userPermissionStore and use the permissionCheck property.
This will return if the given permission is assigned to the user or not.

If you take a look at the permission store, you'll notice that once the permissions are loaded in, it will create a nested
structure that mirrors the above UserPermissions object.

While doing that, it will store if the permission has been assigned to the user or not, allowing for static checking across
the front end.

Below is an example of how you can use it.

```ts
import { userPermissionStore } from '@/stores/userPermissionStore.ts

const userPermissionInfo = userPermissionStore()
const permissionCheck = userPermissionInfo.permissionCheck
```

```vue
<Button v-if="permissionCheck.DevDebug.SendTestEmail" label="Test Email" @click="testEmail" />
```

I will note that this is the preferred usage of permissions, to statically check against "permissionCheck.Resource.Action".

The only place where you should need to directly call UserPermissions is in the vue routing.

### Side Notes
Creating and using a v-directive such as v-permission="permission" is not feasible.  You run into issues with how things
render in the dom, a v-directive cannot prevent the dom from rendering, it can only modify it after the fact.

Due to this, you cannot use it on things such as PrimeVue components, eg tabs.

The only way around this is to use v-if="permissionCheck.Resource.Action" on the component itself, thus the evolution of
having a statically defined permission check that is preloaded with the check.

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
