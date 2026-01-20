import { defineStore } from 'pinia'
import axios from 'axios'

import toaster from '@/services/Toasters'
import type { UserRole, UserRolesResponse } from '@/components/admin/roles/assignedUsers/types.ts'
import type { ListItem } from '@/types/ListItem.ts'
import type { AssignedUserForm } from '@/components/admin/roles/assignedUsers/validation/assignedUserValidation.ts'
import { DateTime } from 'luxon'

export const assignedUsersStore
  = defineStore(`assignedUsersStore`, {
    state: () => {
      return {
        userRoles: [] as UserRole[],
        userList: [] as ListItem[],
      }
    },
    actions: {
      getAssignedUsers: async function (roleId: number) {
        const response = await axios.get<UserRolesResponse>(`/admin/roles/${roleId}/users`)
        for (const item of response.data.roles) {
          item.expireDate = item.expireDate ? DateTime.fromISO(`${item.expireDate}`) : null
        }
        this.userRoles = response.data.roles
      },
      getUserList: async function () {
        const response = await axios.get<ListItem[]>(`/admin/users/summary`)
        this.userList = response.data
      },
      addUserToRole: async function (roleId: number, form: AssignedUserForm) {
        await axios.post(`/admin/roles/${roleId}/users`, {
          userId: form.user.id,
          expireDate: form.expireDate?.toFormat('yyyy-LL-dd'),
        })
        await this.getAssignedUsers(roleId)
        toaster.success('Successfully Added User to Role!')
      },
      removeUserFromRole: async function (roleId: number, userId: string) {
        await axios.delete(`/admin/roles/${roleId}/users/${userId}`)
        await this.getAssignedUsers(roleId)
      },
    },
  })
