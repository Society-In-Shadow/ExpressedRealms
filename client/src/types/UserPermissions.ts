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
  Role: {
    Edit: 'role.edit',
    View: 'role.view',
    Create: 'role.create',
    Delete: 'role.delete',
    Assign: 'role.assign',
  } as const,
  DevDebug: {
    View: 'devdebug.view',
    SendTestEmail: 'devdebug.sendtestemail',
    GetFeatureFlag: 'devdebug.getfeatureflag',
    SendDiscordMessage: 'devdebug.senddiscordmessage',
    TestRedis: 'devdebug.testredis',
  } as const,
} as const

type NestedValues<T> = T extends Record<string, unknown> ? NestedValues<T[keyof T]> : T
export type UserPermission = NestedValues<typeof UserPermissions>
