import Layout from '@/components/LoggedInLayout.vue'
import { UserPermissions } from '@/types/UserPermissions.ts'

export const AdminRoutes = {
  path: '/admin',
  component: Layout,
  children: [
    {
      path: 'players',
      name: 'viewPlayers',
      component: () => import('@/components/admin/players/PlayerList.vue'),
      meta: { requirePermission: UserPermissions.Player.View },
    },
    {
      path: 'players/:id',
      name: 'editPlayer',
      component: () => import('@/components/admin/players/PlayerPage.vue'),
      meta: { requirePermission: UserPermissions.Player.View },
    },
    {
      path: 'characters',
      name: 'adminCharacterList',
      component: () => import('./../../components/admin/characterList/AdminCharacterList.vue'),
      meta: { requiredPermission: UserPermissions.CharacterManagement.View },
    },
    {
      path: 'roles',
      name: 'adminRoleList',
      component: () => import('@/components/admin/roles/RoleList.vue'),
      meta: { requiredPermission: UserPermissions.Role.View },
    },
    {
      path: 'roles/:id',
      name: 'editRole',
      component: () => import('@/components/admin/roles/RolePage.vue'),
      meta: { requiredPermission: UserPermissions.Role.View },
    },
    {
      path: 'events',
      name: 'adminEventList',
      component: () => import('./../../components/admin/events/EventList.vue'),
      meta: { requiredPermission: UserPermissions.Event.View },
    },
    {
      path: 'events/:id',
      name: 'editEvent',
      component: () => import('./../../components/admin/events/EventPage.vue'),
      meta: { requiredPermission: UserPermissions.Event.View },
    },
    {
      path: 'dev',
      name: 'dev',
      component: () => import('@/components/admin/dev/DevTesting.vue'),
      meta: { requiredPermission: UserPermissions.DevDebug.View },
    },
    {
      path: 'archetypes',
      name: 'archetypes',
      component: () => import('@/components/admin/archetypes/ArchetypeList.vue'),
      meta: { requiredPermission: UserPermissions.Archetypes.View },
    },
  ],
}
