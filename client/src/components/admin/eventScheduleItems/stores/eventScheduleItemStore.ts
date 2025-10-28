import { defineStore } from 'pinia'
import axios from 'axios'

import toaster from '@/services/Toasters'
import type { EditEventScheduleItem, EventScheduleItem } from '@/components/admin/eventScheduleItems/types'
import type {
  EventScheduleItemForm,
} from '@/components/admin/eventScheduleItems/validations/eventScheduleItemValidations'
import type { EventScheduleItemResponse } from '@/components/admin/eventScheduleItems/types.ts'

export const EventScheduleItemStore
  = defineStore(`EventScheduleItems`, {
    state: () => {
      return {
        eventScheduleItems: {} as Record<number, EventScheduleItem[]>,
      }
    },
    actions: {
      async getEventScheduleItems(eventId: number) {
        const response = await axios.get<EventScheduleItemResponse>(`/events/${eventId}/scheduleItems/`)
        for (const item of response.data.eventScheduleItems) {
          item.startTime = new Date(item.startTime)
          item.endTime = new Date(item.endTime)
        }
        this.eventScheduleItems[eventId] = response.data.eventScheduleItems
      },
      eventScheduleIsLoaded: function (eventId: number) {
        return this.eventScheduleItems[eventId] != undefined
      },
      getEventScheduleItem: async function (eventId: number, id: number): Promise<EditEventScheduleItem> {
        return this.eventScheduleItems[eventId].find((x: EventScheduleItem) => x.id == id) as EventScheduleItem
      },
      updateEventScheduleItem: async function (eventId: number, values: EventScheduleItemForm, id: number): Promise<void> {
        await axios.put(`/events/${eventId}/scheduleItems/${id}`, {
          name: values.name,
          startDate: values.startDate,
          endDate: values.endDate,
          location: values.location,
          websiteName: values.websiteName,
          websiteUrl: values.websiteUrl,
          additionalNotes: values.additionalNotes,
          timeZoneId: values.timeZone.id,
          conExperience: values.conExperience,
        })
          .then(async () => {
            await this.getEventScheduleItems(eventId)
            toaster.success('Successfully Updated Scheduled Item!')
          })
      },
      addEventScheduleItem: async function (eventId: number, values: EventScheduleItemForm): Promise<void> {
        await axios.post(`/events/${eventId}/scheduleItems/`, {
          name: values.name,
          startDate: values.startDate.toISOString().split('T')[0],
          endDate: values.endDate.toISOString().split('T')[0],
          location: values.location,
          websiteName: values.websiteName,
          websiteUrl: values.websiteUrl,
          additionalNotes: values.additionalNotes,
          timeZoneId: values.timeZone.id,
          conExperience: values.conExperience,
        })
          .then(async () => {
            await this.getEventScheduleItems(eventId)
            toaster.success('Successfully Added Scheduled Item!')
          })
      },
      deleteEventScheduleItem: async function (eventId: number, id: number) {
        let name = 'Scheduled Item'

        const item = this.eventScheduleItems[eventId].find((x: EventScheduleItem) => x.id == id)
        if (item)
          name = item.description

        await axios.delete(`/events/${eventId}/scheduleItems/${id}`)
          .then(async () => {
            await this.getEventScheduleItems(eventId)
            toaster.success(`Successfully Deleted ${name}!`)
          })
      },
    },
  })
