import { defineStore } from 'pinia'
import axios from 'axios'

import type { AssignedXpInfo, AssignedXpResponse } from '@/components/admin/events/types'
import { DateTime } from 'luxon'

export const characterActivityStore
  = defineStore(`CharacterActivity`, {
    state: () => {
      return {
        hasItems: false,
        characterEvents: [] as AssignedXpInfo[],
      }
    },
    actions: {
      async getCharacterEvents(eventId: number) {
        const response = await axios.get<AssignedXpResponse>(`/events/${eventId}/assignedXp`)

        for (const item of response.data.assignedXpInfo) {
          item.dateAssigned = DateTime.fromISO(`${item.dateAssigned}`)
        }
        this.characterEvents = response.data.assignedXpInfo
        this.hasItems = true
      },
    },
  })
