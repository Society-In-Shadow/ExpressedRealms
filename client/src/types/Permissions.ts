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
  Role: {
    Edit: 'role.edit',
    View: 'role.view',
    Create: 'role.create',
    Delete: 'role.delete',
    Assign: 'role.assign',
  } as const,
} as const

export type Permission = typeof Permissions[keyof typeof Permissions]
