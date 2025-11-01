import { defineStore } from 'pinia'
import axios from 'axios'

import type { ListItem } from '@/types/ListItem'
import toaster from '@/services/Toasters'
import type { EditEvent, Event } from '@/components/admin/events/types'
import type { EventForm } from '@/components/admin/events/validations/eventValidations'
import type { EventResponse } from '@/components/admin/events/types.ts'
import { DateTime } from 'luxon'

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

        for (const item of response.data.events) {
          item.startDate = DateTime.fromISO(`${item.startDate}`)
          item.endDate = DateTime.fromISO(`${item.endDate}`)
        }
        this.events = response.data.events

        this.timeZones = [
          {
            id: 'America/New_York',
            name: 'Eastern Time Zone',
            description: 'Eastern Standard Time (EST) / Eastern Daylight Time (EDT)',
          },
          {
            id: 'America/Chicago',
            name: 'Central Time Zone',
            description: 'Central Standard Time (CST) / Central Daylight Time (CDT)',
          },
          {
            id: 'America/Denver',
            name: 'Mountain Time Zone',
            description: 'Mountain Standard Time (MST) / Mountain Daylight Time (MDT)',
          },
          {
            id: 'America/Los_Angeles',
            name: 'Pacific Time Zone',
            description: 'Pacific Standard Time (PST) / Pacific Daylight Time (PDT)',
          },
          {
            id: 'America/Anchorage',
            name: 'Alaska Time Zone',
            description: 'Alaska Standard Time (AKST) / Alaska Daylight Time (AKDT)',
          },
          {
            id: 'Pacific/Honolulu',
            name: 'Hawaii-Aleutian Time Zone',
            description: 'Hawaii-Aleutian Standard Time (HST) / Hawaii-Aleutian Daylight Time (HDT, rarely used)',
          },
        ]
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
            await this.getEvents()
            toaster.success('Successfully Updated Event!')
          })
      },
      addEvent: async function (values: EventForm): Promise<void> {
        await axios.post(`/events/`, {
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

      publishEvent: async function (id: number) {
        let name = 'Event'

        const Event = this.events.find((x: Event) => x.id == id)
        if (Event)
          name = Event.name

        await axios.post(`/events/${id}/publish`)
          .then(async () => {
            await this.getEvents()
            toaster.success(`Successfully Published ${name}!`)
          })
      },
    },
  })
