import { createRouter, createWebHistory } from 'vue-router'
import { type FeatureFlag, type UserRole, userStore } from '@/stores/userStore'
import { UserRoutes } from '@/router/Routes/UserRoutes'
import { AdminRoutes } from '@/router/Routes/AdminRoutes'
import { OverallRoutes } from '@/router/Routes/OverallNavigationRoutes'
import { PublicRoutes } from '@/router/Routes/PublicRoutes'
import { type UserPermission } from '@/types/UserPermissions.ts'

export const routes = [
  PublicRoutes,
  UserRoutes,
  OverallRoutes,
  AdminRoutes,
]

const routerSetup = createRouter({
  history: createWebHistory(),
  routes,
})

let routesInitialized = false
let userInfoInitialized = false

routerSetup.beforeEach(async (to) => {
  const userInfo = userStore()

  const loggedIn = userInfo.isLoggedIn()
  const routeName: string = to.name as string

  // Initialize routes on first navigation
  if (!routesInitialized) {
    await userInfo.updateUserFeatureFlags()

    routesInitialized = true

    // Re-trigger navigation to the same route
    return to
  }

  if (loggedIn) {
    if (!userInfoInitialized) {
      await userInfo.getUserInfo()
      await userInfo.updateUserRoles()
      await userInfo.updateUserPermissions()
      userInfoInitialized = true
    }

    if (userInfo.hasStepsToComplete()) {
      if (userInfo.userNextStepUrl(routeName) == routeName)
        return

      return { name: userInfo.userNextStepUrl(routeName) }
    }

    // if they are on the login page, redirect them to the characters page
    // Also, if they landed on a setup page, redirect them to characters as the
    // above statement should have handled that redirect
    if (to.meta.isUserSetup) {
      return { name: 'characters' }
    }

    if (to.meta.requiredRole && !userInfo.userRoles.includes(to.meta.requiredRole as UserRole)) {
      return { name: 'characters' }
    }

    if (to.meta.requiredPermission && !await userInfo.hasPermission(to.meta.requiredPermission as UserPermission)) {
      return { name: 'characters' }
    }

    if (to.meta.requiredFeatureFlag && !await userInfo.hasFeatureFlag(to.meta.requiredFeatureFlag as FeatureFlag)) {
      return { name: 'characters' }
    }
  }

  if (to.meta.isAnonymous)
    return

  if (!loggedIn) {
    return { name: 'Login' }
  }
})

export default routerSetup
