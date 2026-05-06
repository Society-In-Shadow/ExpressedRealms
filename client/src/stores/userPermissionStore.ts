import { defineStore } from 'pinia'
import axios from 'axios'
import { type UserPermission, UserPermissions } from '@/types/UserPermissions.ts'

type PermissionCheck = {
  [K in keyof typeof UserPermissions]: {
    [P in keyof typeof UserPermissions[K]]: boolean
  }
}

export const userPermissionStore
  = defineStore('userPermissions', {
    state: () => {
      return {
        userPermissions: [] as UserPermission[],
        loadedPermissions: false as boolean,
        permissionCheck: {} as PermissionCheck,
      }
    },
    actions: {
      async updateUserPermissions() {
        const response = await axios.get('/player/permissions')
        this.userPermissions = response.data

        // Build reactive nested object
        this.permissionCheck = Object.fromEntries(
          Object.entries(UserPermissions).map(([category, perms]) => [
            category,
            Object.fromEntries(
              Object.entries(perms).map(([permName, permValue]) => [
                permName,
                this.userPermissions.includes(permValue),
              ]),
            ),
          ]),
        ) as PermissionCheck

        this.loadedPermissions = true
      },

      hasPermission(permission: UserPermission): boolean {
        if (!this.loadedPermissions) {
          if (import.meta.env.DEV) {
            throw new Error(
              '[PermissionStore] hasPermission called before permissions were loaded',
            )
          }
          return false
        }
        return this.userPermissions.includes(permission)
      },

    },
  })

export const can = new Proxy({} as PermissionCheck, {
  get(_, category: keyof PermissionCheck) {
    return new Proxy({} as PermissionCheck[typeof category], {
      get(_, perm: string) {
        const store = userPermissionStore()
        return store.permissionCheck[category][perm]
      },
    })
  },
})
