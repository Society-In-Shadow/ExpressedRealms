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
      meta: { requiredRole: 'UserManagementRole' },
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
      meta: { requiredRole: 'ManagePlayerCharacterList' },
    },
    {
      path: 'roles',
      name: 'adminRoleList',
      component: () => import('@/components/admin/roles/RoleList.vue'),
      meta: { requiredRole: 'UserManagementRole' },
    },
    {
      path: 'roles/:id',
      name: 'editRole',
      component: () => import('@/components/admin/roles/RolePage.vue'),
      meta: { requiredRole: 'UserManagementRole' },
    },
    {
      path: 'events',
      name: 'adminEventList',
      component: () => import('./../../components/admin/events/EventList.vue'),
      meta: { requiredRole: 'ManageEvents' },
    },
    {
      path: 'events/:id',
      name: 'editEvent',
      component: () => import('./../../components/admin/events/EventPage.vue'),
      meta: { requiredRole: 'ManageEvents' },
    },
    {
      path: 'dev',
      name: 'dev',
      component: () => import('@/components/admin/dev/DevTesting.vue'),
      meta: { requiredPermission: UserPermissions.DevDebug.View },
    },
  ],
}
