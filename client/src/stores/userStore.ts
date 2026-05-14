import { defineStore } from 'pinia'
import axios from 'axios'

export const FeatureFlags = {
  ShowMarketingContactUs: 'show-marketing-contact-us',
  ShowFactionDropdown: 'show-faction-dropdown',
  ShowEventCheckin: 'show-event-checkin',
  ShowArchetypeSelection: 'show-archetype-selection',
} as const

export type FeatureFlag = (typeof FeatureFlags)[keyof typeof FeatureFlags]

export const userStore
  = defineStore('user', {
    state: () => {
      return {
        userFeatureFlags: [] as string[],
        lastFeatureFlagLoad: null as Date | null,
      }
    },
    actions: {
      async updateUserFeatureFlags() {
        if (this.lastFeatureFlagLoad != null && this.lastFeatureFlagLoad.getTime() > new Date().getTime() - 300_000) {
          return
        }
        this.lastFeatureFlagLoad = new Date()
        return await axios.get('/navMenu/featureFlags')
          .then((response) => {
            this.userFeatureFlags = response.data.featureFlags
          })
      },
      async hasFeatureFlag(featureFlag: FeatureFlag): Promise<boolean> {
        await this.updateUserFeatureFlags()
        return this.userFeatureFlags.includes(featureFlag)
      },
    },
  })
