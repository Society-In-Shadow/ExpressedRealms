import { defineStore } from 'pinia'
import axios from 'axios'
import { type UserPermission, UserPermissions } from '@/types/UserPermissions.ts'

export const userPermissionStore
  = defineStore('userPermissions', {
    state: () => {
      return {
        userPermissions: [] as UserPermission[],
        loadedPermissions: false as boolean,
        permissionCheck: {} as Record<string, any>,
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
        )

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
