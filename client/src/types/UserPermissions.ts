/**
 * Auto-Generated, Do Not Edit
 */
export const UserPermissions = {
  CharacterContacts: {
    Approve: 'charactercontacts.approve',
  } as const,
  EventQuestion: {
    Edit: 'eventquestion.edit',
    View: 'eventquestion.view',
    Create: 'eventquestion.create',
    Delete: 'eventquestion.delete',
  } as const,
  Event: {
    Edit: 'event.edit',
    View: 'event.view',
    Create: 'event.create',
    Delete: 'event.delete',
    Publish: 'event.publish',
    Checkin: 'event.checkin',
    DownloadConSummaryReport: 'event.downloadconsummaryreport',
  } as const,
  EventScheduleItem: {
    Edit: 'eventscheduleitem.edit',
    View: 'eventscheduleitem.view',
    Create: 'eventscheduleitem.create',
    Delete: 'eventscheduleitem.delete',
  } as const,
  Player: {
    Edit: 'player.edit',
    View: 'player.view',
    Disable: 'player.disable',
    Enable: 'player.enable',
    BypassEmailConfirmation: 'player.bypassemailconfirmation',
    BypassLockout: 'player.bypasslockout',
    ManageRoles: 'player.manageroles',
    ViewActivityLogs: 'player.viewactivitylogs',
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
    RunSpecialScripts: 'devdebug.runspecialscripts',
  } as const,
} as const

type NestedValues<T> = T extends Record<string, unknown> ? NestedValues<T[keyof T]> : T
export type UserPermission = NestedValues<typeof UserPermissions>
