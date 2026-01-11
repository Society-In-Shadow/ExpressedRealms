import { defineStore } from 'pinia'
import axios from 'axios'

import toaster from '@/services/Toasters'
import type { EditRole, Role, RoleResponse } from '@/components/admin/roles/types.ts'
import type { RoleForm } from '@/components/admin/roles/validations/roleValidations.ts'

export const RoleStore
  = defineStore(`Role`, {
    state: () => {
      return {
        haveEventTypes: false,
        haveRoles: false,
        roles: [] as Role[],
      }
    },
    actions: {
      async getRoles() {
        const response = await axios.get<RoleResponse>(`/admin/roles`)

        this.roles = response.data.roles

        this.haveRoles = true
      },
      getEvent: async function (id: number): Promise<EditRole> {
        const response = await axios.get<Role>(`/admin/roles/${id}`)

        return {
          ...response.data,
        }
      },
      updateEvent: async function (values: RoleForm, id: number): Promise<void> {
        await axios.put(`/admin/roles/${id}`, {
          name: values.name,
          description: values.description,
          permissions: [],
        })
          .then(async () => {
            await this.getRoles()
            toaster.success('Successfully Updated Role!')
          })
      },
      addEvent: async function (values: RoleForm): Promise<void> {
        await axios.post(`/admin/roles/`, {
          name: values.name,
          description: values.description,
          permissions: [],
        })
          .then(async () => {
            await this.getRoles()
            toaster.success('Successfully Added Role!')
          })
      },
      deleteEvent: async function (id: number) {
        let name = 'Role'

        const Event = this.roles.find((x: Role) => x.id == id)
        if (Event)
          name = Event.name

        await axios.delete(`/admin/roles/${id}`)
          .then(async () => {
            await this.getRoles()
            toaster.success(`Successfully Deleted ${name}!`)
          })
      },
    },
  })
