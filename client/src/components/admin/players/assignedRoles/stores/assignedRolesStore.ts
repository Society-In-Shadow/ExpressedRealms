import { defineStore } from 'pinia'
import axios from 'axios'

import toaster from '@/services/Toasters'
import type { UserRole, UserRolesResponse } from '@/components/admin/players/assignedRoles/types.ts'
import type { ListItem } from '@/types/ListItem.ts'
import type { AssignedRoleForm } from '@/components/admin/players/assignedRoles/validation/assignedUserValidation.ts'
import { DateTime } from 'luxon'

export const assignedRolesStore
  = defineStore(`assignedRolesStore`, {
    state: () => {
      return {
        userRoles: [] as UserRole[],
        roleList: [] as ListItem[],
      }
    },
    actions: {
      getAssignedRoles: async function (userId: string) {
        const response = await axios.get<UserRolesResponse>(`/admin/users/${userId}/roles`)
        for (const item of response.data.roles) {
          item.expireDate = item.expireDate ? DateTime.fromISO(`${item.expireDate}`) : null
        }
        this.userRoles = response.data.roles
      },
      getRoleList: async function () {
        const response = await axios.get<ListItem[]>(`/admin/roles/summary`)
        this.roleList = response.data
      },
      addRoleToUser: async function (userId: string, form: AssignedRoleForm) {
        await axios.post(`/admin/users/${userId}/roles`, {
          roleId: form.role.id,
          expireDate: form.expireDate?.toFormat('yyyy-LL-dd'),
        })
        await this.getAssignedRoles(userId)
        toaster.success('Successfully Added Role to User!')
      },
      removeRoleFromUser: async function (roleId: number, userId: string) {
        await axios.delete(`/admin/users/${userId}/roles/${userId}`)
        await this.getAssignedRoles(userId)
      },
    },
  })
