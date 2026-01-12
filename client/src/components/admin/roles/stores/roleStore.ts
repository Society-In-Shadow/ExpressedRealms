import { defineStore } from 'pinia'
import axios from 'axios'

import toaster from '@/services/Toasters'
import type { EditRole, Resource, ResourceResponse, Role, RoleResponse } from '@/components/admin/roles/types.ts'
import type { RoleForm } from '@/components/admin/roles/validations/roleValidations.ts'

export const RoleStore
  = defineStore(`Role`, {
    state: () => {
      return {
        haveEventTypes: false,
        haveRoles: false,
        haveRole: false,
        roles: [] as Role[],
        resources: [] as Resource[],
        role: {} as EditRole,
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
        this.role = response.data
        if (this.resources.length === 0) {
          const resources = await axios.get<ResourceResponse>(`/admin/roles/permissions`)
          this.resources = resources.data.resources
        }
        this.haveRole = true
        return this.role
      },
      updateEvent: async function (): Promise<void> {
        await axios.put(`/admin/roles/${this.role.id}`, {
          name: this.role.name,
          description: this.role.description,
          permissionIds: this.role.permissionIds,
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
          permissionIds: [],
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
