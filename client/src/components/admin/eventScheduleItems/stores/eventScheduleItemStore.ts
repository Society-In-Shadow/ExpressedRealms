import { defineStore } from 'pinia'
import axios from 'axios'

import toaster from '@/services/Toasters'
import type { EditEventScheduleItem, EventScheduleItem } from '@/components/admin/eventScheduleItems/types'
import type {
  EventScheduleItemForm,
} from '@/components/admin/eventScheduleItems/validations/eventScheduleItemValidations'
import type { EventScheduleItemResponse } from '@/components/admin/eventScheduleItems/types.ts'
import { DateTime } from 'luxon'

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
          item.startTime = DateTime.fromISO(item.startTime)
          item.endTime = DateTime.fromISO(item.endTime)
          item.date = DateTime.fromISO(item.date)
        }
        this.eventScheduleItems[eventId] = response.data.eventScheduleItems
      },
      getEventScheduleItem: async function (eventId: number, id: number): Promise<EditEventScheduleItem> {
        return this.eventScheduleItems[eventId].find((x: EventScheduleItem) => x.id == id) as EventScheduleItem
      },
      updateEventScheduleItem: async function (eventId: number, values: EventScheduleItemForm, id: number): Promise<void> {
        await axios.put(`/events/${eventId}/scheduleItems/${id}`, {
          description: values.description,
          startTime: values.startTime.toFormat('yyyy-MM-dd\'T\'HH:mm:ss'),
          endTime: values.endTime.toFormat('yyyy-MM-dd\'T\'HH:mm:ss'),
          date: values.date.toFormat('yyyy-LL-dd'),
        })
          .then(async () => {
            await this.getEventScheduleItems(eventId)
            toaster.success('Successfully Updated Scheduled Item!')
          })
      },
      addEventScheduleItem: async function (eventId: number, values: EventScheduleItemForm): Promise<void> {
        await axios.post(`/events/${eventId}/scheduleItems/`, {
          description: values.description,
          startTime: values.startTime.toFormat('yyyy-MM-dd\'T\'HH:mm:ss'),
          endTime: values.endTime.toFormat('yyyy-MM-dd\'T\'HH:mm:ss'),
          date: values.date.toFormat('yyyy-LL-dd'),
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
