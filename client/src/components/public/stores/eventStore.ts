import {defineStore} from 'pinia'
import type {EventDetails} from '@/components/public/types'

export const eventStore
  = defineStore(`events`, {
    state: () => {
      return {
        events: [] as EventDetails[],
      }
    },
    actions: {
      async getEvents() {
        const events = [{
          id: 1,
          name: 'Sioux City Geek Con',
          location: 'Sioux City Convention Center Sioux City, Iowa',
          startDate: new Date(2025, 7, 22),
          endDate: new Date(2025, 7, 24),
          conWebsiteName: 'Sioux City Table Top RPG',
          conWebsiteUrl: 'https://tabletop.events/conventions/sioux-city-geek-con-fall-2025',
        }, {
          id: 2,
          name: 'Nuke-Con',
          location: 'Mid-America Center, Council Bluffs, IA 51501',
          startDate: new Date(2025, 9, 3),
          endDate: new Date(2025, 9, 5),
          conWebsiteName: 'Nuke-Con',
          conWebsiteUrl: 'https://www.nuke-con.com/',
        }]

        this.events = events.filter(event => event.endDate >= new Date())
      },
    },
  })
