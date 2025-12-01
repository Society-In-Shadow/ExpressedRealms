import Layout from '@/components/LoggedInLayout.vue'

export const AdminRoutes = {
  path: '/admin',
  component: Layout,
  children: [
    {
      path: 'players',
      name: 'viewPlayers',
      component: () => import('./../../components/players/PlayerList.vue'),
      meta: { requiredRole: 'UserManagementRole' },
    },
    {
      path: 'characters',
      name: 'adminCharacterList',
      component: () => import('./../../components/admin/characterList/AdminCharacterList.vue'),
      meta: { requiredRole: 'ManagePlayerCharacterList' },
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
  ],
}
