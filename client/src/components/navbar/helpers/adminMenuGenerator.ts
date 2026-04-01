import { userPermissionStore } from '@/stores/userPermissionStore.ts'

export function populateAdminMenu() {
  const permissionInfo = userPermissionStore()
  const permissionCheck = permissionInfo.permissionCheck
  const adminMenuItems = []

  if (permissionCheck.Player.View) {
    adminMenuItems.push(
      {
        navMenuType: 'simple',
        label: 'Users',
        navMenuIcon: 'groups',
        pushComponentRouteName: 'viewPlayers',
        description: 'Manage all users in the system.',
        visible: () => permissionCheck.Player.View,
      })
  }

  if (permissionCheck.Event.View) {
    adminMenuItems.push({
      navMenuType: 'simple',
      label: 'Events',
      navMenuIcon: 'calendar_month',
      pushComponentRouteName: 'adminEventList',
      description: 'Manage all events in the system.',
      visible: () => permissionCheck.Event.View,
    })
  }

  if (permissionCheck.Role.View) {
    adminMenuItems.push(
      {
        navMenuType: 'simple',
        label: 'Role Management',
        description: 'Manage Roles that users can have.',
        navMenuIcon: 'group',
        pushComponentRouteName: 'adminRoleList',
        visible: () => permissionCheck.Role.View,
      })
  }

  if (permissionCheck.CharacterManagement.View) {
    adminMenuItems.push({
      navMenuType: 'simple',
      label: 'Character Management',
      description: 'Manage any primary characters across all players.',
      navMenuIcon: 'patient_list',
      pushComponentRouteName: 'adminCharacterList',
      visible: () => permissionCheck.CharacterManagement.View,
    })
  }

  if (permissionCheck.DevDebug.View) {
    adminMenuItems.push({
      navMenuType: 'simple',
      label: 'Dev Menu',
      description: 'Dev testing / related functionality.',
      navMenuIcon: 'code',
      pushComponentRouteName: 'dev',
      visible: () => permissionCheck.DevDebug.View,
    })
  }

  if (permissionCheck.Archetypes.View) {
    adminMenuItems.push({
      navMenuType: 'simple',
      label: 'Archetypes',
      description: 'Archetypes are default characters.',
      navMenuIcon: 'architecture',
      pushComponentRouteName: 'archetypes',
      visible: () => permissionCheck.Archetypes.View,
    })
  }

  return adminMenuItems
}
