import { defineStore } from 'pinia'
import axios from 'axios'
import toaster from '@/services/Toasters'
import type { EventForm } from '@/components/admin/events/validations/eventValidations'
import { DateTime } from 'luxon'
import type { AssignedXpInfo, AssignedXpResponse, EditAssignedXpItem } from '@/components/admin/assignedXp/types.ts'
import type { ListItem } from '@/types/ListItem.ts'

export const AssignedXpStore
  = defineStore(`assignedXp`, {
    state: () => {
      return {
        hasItems: false,
        assignedXpItems: [] as AssignedXpInfo[],
        events: [] as ListItem[],
      }
    },
    actions: {
      async getAssignedXp(characterId: number) {
        const response = await axios.get<AssignedXpResponse>(`/characters/${characterId}/assignedXp`)
        const eventResponse = await axios.get<ListItem[]>(`/events/summary`)

        for (const item of response.data.assignedXpInfo) {
          item.dateAssigned = DateTime.fromISO(`${item.dateAssigned}`)
        }
        this.assignedXpItems = response.data.assignedXpInfo
        this.events = eventResponse.data
        this.hasItems = true
      },
      getEvent: async function (id: number): Promise<EditAssignedXpItem> {
        const item = this.assignedXpItems.find((x: AssignedXpInfo) => x.id == id)

        return {
          ...item,
          item: this.events.find((x: ListItem) => x.id == item!.event.id) as ListItem,
        }
      },
      updateEvent: async function (values: EventForm, id: number, characterId: number): Promise<void> {
        await axios.put(`/characters/${characterId}/assignedXp/${id}`, {
          name: values.name,
          startDate: values.startDate.toFormat('yyyy-LL-dd'),
          endDate: values.endDate.toFormat('yyyy-LL-dd'),
          location: values.location,
          websiteName: values.websiteName,
          websiteUrl: values.websiteUrl,
          additionalNotes: values.additionalNotes,
          timeZoneId: values.timeZone.id,
          conExperience: values.conExperience,
        })
          .then(async () => {
            await this.getAssignedXp(characterId)
            toaster.success('Successfully Updated Event!')
          })
      },
      addEvent: async function (values: EventForm, characterId: number): Promise<void> {
        await axios.post(`/characters/${characterId}/assignedXp/`, {
          name: values.name,
          startDate: values.startDate.toFormat('yyyy-LL-dd'),
          endDate: values.endDate.toFormat('yyyy-LL-dd'),
          location: values.location,
          websiteName: values.websiteName,
          websiteUrl: values.websiteUrl,
          additionalNotes: values.additionalNotes,
          timeZoneId: values.timeZone.id,
          conExperience: values.conExperience,
        })
          .then(async () => {
            await this.getAssignedXp(characterId)
            toaster.success('Successfully Added Event!')
          })
      },
      deleteEvent: async function (characterId: number, id: number) {
        await axios.delete(`/characters/${characterId}/assignedXp/${id}`)
          .then(async () => {
            await this.getAssignedXp(characterId)
            toaster.success(`Successfully Deleted the Assigned XP!`)
          })
      },
    },
  })
