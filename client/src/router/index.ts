import { createRouter, createWebHistory } from 'vue-router'
import { type FeatureFlag, userStore } from '@/stores/userStore'
import { UserRoutes } from '@/router/Routes/UserRoutes'
import { AdminRoutes } from '@/router/Routes/AdminRoutes'
import { OverallRoutes } from '@/router/Routes/OverallNavigationRoutes'
import { PublicRoutes } from '@/router/Routes/PublicRoutes'
import { type UserPermission } from '@/types/UserPermissions.ts'
import { userPermissionStore } from '@/stores/userPermissionStore.ts'
import { useQueryCache } from '@pinia/colada'
import { userInfoQuery } from '@/auth/authStore.ts'
import { SetupState } from '@/auth/types.ts'
import { useQueryWithLoading } from '@/utilities/queryOverride.ts'

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
  const userPermissions = userPermissionStore()

  const queryCache = useQueryCache()
  await queryCache.refresh(queryCache.ensure(userInfoQuery))
  const { data } = useQueryWithLoading(userInfoQuery)

  const loggedIn = data.value?.userInfo !== null
  const routeName: string = to.name as string

  // Initialize routes on first navigation
  if (!routesInitialized) {
    await userInfo.updateUserFeatureFlags()

    routesInitialized = true

    // Re-trigger navigation to the same route
    return to
  }

  if (loggedIn) {
    if (data.value!.userInfo!.setupState == SetupState.UnconfirmedEmail
      && routeName !== 'pleaseConfirmEmail' && routeName !== 'confirmAccount') {
      return { name: 'pleaseConfirmEmail' }
    }

    if (data.value!.userInfo!.setupState == SetupState.SetProfileName
      && routeName !== 'setupProfile') {
      return { name: 'setupProfile' }
    }

    if (!userInfoInitialized) {
      await userPermissions.updateUserPermissions()
      userInfoInitialized = true
    }

    if (to.meta.isUserSetup && data.value!.userInfo!.setupState == SetupState.Done) {
      return { name: 'characters' }
    }

    if (to.meta.requiredPermission && !await userPermissions.hasPermission(to.meta.requiredPermission as UserPermission)) {
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
