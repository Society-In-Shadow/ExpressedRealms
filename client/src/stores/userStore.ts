import { defineStore } from 'pinia'
import axios from 'axios'

export const UserRoles = {
  ExpressionEditor: 'ExpressionEditorRole',
  KnowledgeManagementRole: 'KnowledgeManagementRole',
  BlessingsManagementRole: 'ManageBlessingsRole',
  ManageModifiers: 'ManageModifiers',
} as const

export const FeatureFlags = {
  ShowMarketingContactUs: 'show-marketing-contact-us',
  ShowFactionDropdown: 'show-faction-dropdown',
  ShowEventCheckin: 'show-event-checkin',
  ShowArchetypeSelection: 'show-archetype-selection',
} as const

export type UserRole = (typeof UserRoles)[keyof typeof UserRoles]
export type FeatureFlag = (typeof FeatureFlags)[keyof typeof FeatureFlags]

export const userStore
  = defineStore('user', {
    state: () => {
      return {
        userRoles: [] as string[],
        userFeatureFlags: [] as string[],
        lastFeatureFlagLoad: null as Date | null,
        lastRoleLoad: null as Date | null,
      }
    },
    actions: {
      async updateUserRoles() {
        if (this.lastRoleLoad != null && this.lastRoleLoad.getTime() > new Date().getTime() - 300_000) {
          return
        }
        this.lastRoleLoad = new Date()
        await axios.get('/navMenu/permissions')
          .then((response) => {
            this.userRoles = response.data.roles
          })
      },
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
      async hasUserRole(role: UserRole): Promise<boolean> {
        await this.updateUserRoles()
        return this.userRoles.includes(role)
      },
      async hasFeatureFlag(featureFlag: FeatureFlag): Promise<boolean> {
        await this.updateUserFeatureFlags()
        return this.userFeatureFlags.includes(featureFlag)
      },
    },
  })
