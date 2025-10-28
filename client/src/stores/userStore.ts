import { defineStore } from 'pinia'
import axios from 'axios'
import { updateUserStoreWithEmailInfo, updateUserStoreWithPlayerInfo } from '@/services/Authentication'

export const UserRoles = {
  ExpressionEditor: 'ExpressionEditorRole',
  UserManagementRole: 'UserManagementRole',
  PowerManagementRole: 'PowerManagementRole',
  KnowledgeManagementRole: 'KnowledgeManagementRole',
  DownloadCMSReports: 'DownloadCMSReports',
  DownloadExpressionBooklet: 'DownloadExpressionBooklet',
  BlessingsManagementRole: 'ManageBlessingsRole',
  ManagePlayerCharacterList: 'ManagePlayerCharacterList',
  ManageProgressionPaths: 'ManageProgressionPaths',
  ManageModifiers: 'ManageModifiers',
  ManageEventRole: 'ManageEvents',
} as const

export const FeatureFlags = {
  ShowMarketingContactUs: 'show-marketing-contact-us',
  ShowFactionDropdown: 'show-faction-dropdown',
} as const

export type UserRole = (typeof UserRoles)[keyof typeof UserRoles]
export type FeatureFlag = (typeof FeatureFlags)[keyof typeof FeatureFlags]

export const userStore
  = defineStore('user', {
    state: () => {
      return {
        userEmail: '' as string,
        name: '' as string,
        hasConfirmedEmail: false as boolean,
        isPlayerSetup: false as boolean,
        userRoles: [] as string[],
        userFeatureFlags: [] as string[],
        loadedUserInfo: false as boolean,
        lastFeatureFlagLoad: null as Date | null,
        lastPermissionLoad: null as Date | null,
        lastAuthCheck: null as Date | null,
        isLoggedInCache: false as boolean,
      }
    },
    actions: {
      isLoggedIn() {
        const self = this
        if (this.lastAuthCheck != null && this.lastAuthCheck.getTime() > new Date().getTime() - 600_000) {
          return this.isLoggedInCache
        }
        this.lastAuthCheck = new Date()
        axios.get('/auth/check')
          .then((response) => {
            self.isLoggedInCache = true
          })
          .catch((error) => {
            self.isLoggedInCache = false
          })
        return this.isLoggedInCache
      },
      async updateLoggedInStatus() {
        const self = this
        this.lastAuthCheck = new Date()
        await axios.get('/auth/check')
          .then((response) => {
            self.isLoggedInCache = true
          })
          .catch((error) => {
            self.isLoggedInCache = false
          })
        return this.isLoggedInCache
      },
      async updateUserRoles() {
        if (this.lastPermissionLoad != null && this.lastPermissionLoad.getTime() > new Date().getTime() - 300_000) {
          return
        }
        this.lastPermissionLoad = new Date()
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
      async getUserInfo() {
        await updateUserStoreWithPlayerInfo()
        await updateUserStoreWithEmailInfo()
      },
      hasStepsToComplete(): boolean {
        // User needs email confirmed
        if (!this.hasConfirmedEmail) {
          return true
        }

        // User Needs profile setup
        return !this.isPlayerSetup
      },
      userNextStepUrl(nextStep: string): string {
        // confirm account should stay on the same page if the email hasn't been confirmed
        if (nextStep == 'confirmAccount' && !this.hasConfirmedEmail)
          return 'confirmAccount'

        if (!this.hasConfirmedEmail) {
          return 'pleaseConfirmEmail'
        }

        return 'setupProfile'
      },
    },
  })
