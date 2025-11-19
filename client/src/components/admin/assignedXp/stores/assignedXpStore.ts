import { defineStore } from 'pinia'
import axios from 'axios'
import toaster from '@/services/Toasters'
import type { EventForm } from '@/components/admin/events/validations/eventValidations'
import { DateTime } from 'luxon'
import type { AssignedXpInfo, AssignedXpResponse, EditAssignedXpItem } from '@/components/admin/assignedXp/types.ts'
import type { ListItem } from '@/types/ListItem.ts'
import type { AssignedXpForm } from '@/components/admin/assignedXp/validations/assignedXpValidations.ts'

export const AssignedXpStore
  = defineStore(`assignedXp`, {
    state: () => {
      return {
        hasItems: false,
        assignedXpItems: [] as AssignedXpInfo[],
        events: [] as ListItem[],
        xpTypes: [] as ListItem[],
      }
    },
    actions: {
      async getAssignedXp(characterId: number) {
        this.hasItems = false
        const response = await axios.get<AssignedXpResponse>(`/characters/${characterId}/assignedXp`)
        const eventResponse = await axios.get<ListItem[]>(`/events/summary`)

        for (const item of response.data.assignedXpInfo) {
          item.dateAssigned = DateTime.fromISO(`${item.dateAssigned}`)
        }
        this.assignedXpItems = response.data.assignedXpInfo
        this.events = eventResponse.data
        this.hasItems = true
        this.xpTypes = [
          {
            id: 2,
            name: 'Check-in Bonus',
            description: 'XP earned when they initially check in',
          },
          {
            id: 3,
            name: 'Award XP',
            description: 'XP assigned out for best costume, etc',
          },
          {
            id: 4,
            name: 'First Time Player XP',
            description: 'First time players will get max of 5 XP',
          },
          {
            id: 5,
            name: 'Brought New Player XP',
            description: 'Player introduced new player, will get max XP',
          },
          {
            id: 6,
            name: 'Other',
            description: 'XP is being assigned out for uncommon reason',
          },
        ]
        this.hasItems = true
      },
      getEvent: async function (id: number): Promise<EditAssignedXpItem> {
        const item = this.assignedXpItems.find((x: AssignedXpInfo) => x.id == id)

        return {
          ...item,
          event: this.events.find((x: ListItem) => x.id == item!.event.id) as ListItem,
          xpType: this.xpTypes.find((x: ListItem) => x.id == item!.xpType.id) as ListItem,
        }
      },
      update: async function (values: AssignedXpForm, id: number, characterId: number): Promise<void> {
        await axios.put(`/characters/${characterId}/assignedXp/${id}`, {
          eventId: values.event.id,
          assignedXpTypeId: values.xpType.id,
          reason: values.notes,
          amount: values.amount,
        })
          .then(async () => {
            await this.getAssignedXp(characterId)
            toaster.success('Successfully Updated XP!')
          })
      },
      add: async function (values: EventForm, characterId: number): Promise<void> {
        await axios.post(`/characters/${characterId}/assignedXp/`, {
          eventId: values.event.id,
          assignedXpTypeId: values.xpType.id,
          reason: values.notes,
          amount: values.amount,
        })
          .then(async () => {
            await this.getAssignedXp(characterId)
            toaster.success('Successfully Added XP to Character!')
          })
      },
      delete: async function (characterId: number, id: number) {
        await axios.delete(`/characters/${characterId}/assignedXp/${id}`)
          .then(async () => {
            await this.getAssignedXp(characterId)
            toaster.success(`Successfully Deleted the XP!`)
          })
      },
    },
  })
