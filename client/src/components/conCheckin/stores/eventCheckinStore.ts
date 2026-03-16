import { defineStore } from 'pinia'
import axios from 'axios'
import type {
  ActiveEvent,
  AgeInfo,
  ApproveCheckinInfo,
  AssignedXpType,
  BasicInfo,
  CheckinInfo,
  GetCheckinQuestionsResponse,
  GoCheckinInfo,
  PrimaryCharacterInfo,
  Question,
} from '@/components/conCheckin/types.ts'
import toaster from '@/services/Toasters'

export const EventCheckinStore
  = defineStore(`eventCheckin`, {
    state: () => {
      return {
        foundInfo: false,
        isReset: false,
        hasActiveEvent: false,
        hasInvalidLookupId: false,
        lookupId: '',
        event: {} as ActiveEvent,
        checkinStage: {} as BasicInfo | null,
        goCheckinInfo: {} as GoCheckinInfo,
        playerNumber: 0,
        broughtNewPlayer: null as boolean | null,
        assignedXp: {} as AssignedXpType | null | undefined,
        primaryCharacter: {} as PrimaryCharacterInfo | null,
      }
    },
    actions: {
      async resetGoPage() {
        this.foundInfo = false
        this.isReset = true
        this.hasActiveEvent = false
        this.lookupId = ''
        this.event = {} as ActiveEvent
        this.checkinStage = null
        this.goCheckinInfo = {} as GoCheckinInfo
        this.playerNumber = 0
        this.broughtNewPlayer = false
        this.assignedXp = null
        this.primaryCharacter = null
      },
      async getCheckinAvailable() {
        const response = await axios.get<boolean>(`/events/checkin/available`)
        this.hasActiveEvent = response.data
      },
      async getCheckinInfo() {
        const response = await axios.get<CheckinInfo>(`/events/checkin/info`)

        this.lookupId = response.data.lookupId
        this.event = response.data.event
        this.checkinStage = response.data.checkinStage
      },
      async getGoCheckinInfo(lookupId: string): Promise<boolean> {
        const response = await axios.get<GoCheckinInfo>(`/events/checkin/lookup/${encodeURIComponent(lookupId)}`)

        if (!response.data.wasFound) {
          this.hasInvalidLookupId = true
          return false
        }

        this.hasInvalidLookupId = false
        this.goCheckinInfo = response.data
        return true
      },
      async verifiedUserInfo() {
        this.foundInfo = false
        const response = await axios.get<ApproveCheckinInfo>(`/events/checkin/lookup/${this.lookupId}/approve`)

        this.foundInfo = true
        this.checkinId = response.data.checkinId
        this.playerNumber = response.data.playerNumber
        this.assignedXp = response.data.assignedXp
        this.primaryCharacter = response.data.primaryCharacterInfo
        this.checkinStage = response.data.currentStage
        this.isReset = false
      },
      async verifiedAge(ageTypeId: number, hasWaiver: boolean) {
        await axios.put(`events/checkin/lookup/${this.lookupId}/ageInfo`, {
          ageGroupId: ageTypeId,
          hasSignedConsentForm: hasWaiver,
        })
        await this.resetGoPage()
      },
      async getVerifiedAge(): Promise<AgeInfo> {
        const response = await axios.get<AgeInfo>(`events/checkin/lookup/${this.lookupId}/ageInfo`)
        return response.data
      },
      async getQuestions(): Promise<Question[]> {
        const response = await axios.get<GetCheckinQuestionsResponse>(`events/checkin/lookup/${this.lookupId}/questions`)
        return response.data.questions
      },
      async updateQuestion(question: Question) {
        await axios.put(`/events/checkin/lookup/${this.lookupId}/questions/${question.id}`, { response: question.response })
        toaster.success('Question updated successfully!')
      },
      async addAssignedXp(typeId: number, amount: number) {
        await axios.post(`/events/checkin/lookup/${this.lookupId}/assignXp`, { amount: amount, AssignedXpTypeId: typeId })
        await this.verifiedUserInfo()
        toaster.success('Assigned XP successfully!')
      },
      async approveStage(stageId: number) {
        await axios.post(`/events/checkin/lookup/${this.lookupId}/approveStage`, { stageId: stageId })
        await this.resetGoPage()
        toaster.success('Stage approved successfully!')
      },
      async retireCharacter() {
        await axios.put(`/characters/${this.lookupId}/retire`)
        await this.resetGoPage()
      },
    },
  })
