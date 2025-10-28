import { defineStore } from 'pinia'
import axios from 'axios'

import type { ListItem } from '@/types/ListItem'
import toaster from '@/services/Toasters'
import type { EditEvent, Event } from '@/components/admin/events/types'
import type { EventForm } from '@/components/admin/events/validations/eventValidations'
import type { EventResponse } from '@/components/admin/events/types.ts'

export const EventStore
  = defineStore(`Events`, {
    state: () => {
      return {
        timeZones: [] as ListItem[],
        haveEventTypes: false,
        haveEvents: false,
        events: [] as Event[],
      }
    },
    actions: {
      async getEvents() {
        const response = await axios.get<EventResponse>(`/events`)
        this.events = response.data.events

        this.timeZones = [{ id: 'America/Chicago', name: 'Central Time Zone', description: 'test' } as ListItem]
      },
      getEvent: async function (id: number): Promise<EditEvent> {
        const event = this.events.find((x: Event) => x.id == id)

        return {
          ...event,
          timeZone: this.timeZones.find((x: ListItem) => x.id == event.timeZoneId) as ListItem,
        }
      },
      updateEvent: async function (values: EventForm, id: number): Promise<void> {
        await axios.put(`/events/${id}`, {
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
            await this.getEvents()
            toaster.success('Successfully Updated Event!')
          })
      },
      addEvent: async function (values: EventForm): Promise<void> {
        await axios.post(`/events/`, {
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
            await this.getEvents()
            toaster.success('Successfully Added Event!')
          })
      },
      deleteEvent: async function (id: number) {
        let name = 'Event'

        const Event = this.events.find((x: Event) => x.id == id)
        if (Event)
          name = Event.name

        await axios.delete(`/events/${id}`)
          .then(async () => {
            await this.getEvents()
            toaster.success(`Successfully Deleted ${name}!`)
          })
      },
    },
  })
