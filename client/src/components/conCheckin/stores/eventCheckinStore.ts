import { defineStore } from 'pinia'
import axios from 'axios'
import type { CheckinInfo } from '@/components/conCheckin/types.ts'

export const EventCheckinStore
  = defineStore(`eventCheckin`, {
    state: () => {
      return {
        hasActiveEvent: false,
        lookupId: '',
        eventId: 0,
      }
    },
    actions: {
      async getCheckinAvailable() {
        const response = await axios.get<boolean>(`/events/checkin/available`)
        this.hasActiveEvent = response.data
      },
      async getCheckinInfo() {
        const response = await axios.get<CheckinInfo>(`/events/checkin/info`)

        this.lookupId = response.data.lookupId
        this.eventId = response.data.eventId
      },
    },
  })
