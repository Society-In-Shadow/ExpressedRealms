import { defineStore } from 'pinia'
import axios from 'axios'
import type {
  ApproveCheckinInfo,
  AssignedXpType,
  CheckinInfo,
  GoCheckinInfo,
  Question,
} from '@/components/conCheckin/types.ts'
import toaster from '@/services/Toasters'

export const EventCheckinStore
  = defineStore(`eventCheckin`, {
    state: () => {
      return {
        hasActiveEvent: false,
        lookupId: '',
        eventId: 0,
        goCheckinInfo: {} as GoCheckinInfo,
        checkinId: 0,
        playerNumber: 0,
        questions: [] as Question[],
        broughtNewPlayer: false,
        assignedXp: {} as AssignedXpType | null | undefined,
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
      async getGoCheckinInfo(lookupId: string): Promise<boolean> {
        const response = await axios.get<GoCheckinInfo>(`/events/checkin/lookup/${lookupId}`)

        if (response.status !== 200) return false

        this.goCheckinInfo = response.data
        return true
      },
      async verifiedUserInfo() {
        const response = await axios.get<ApproveCheckinInfo>(`/events/checkin/lookup/${this.lookupId}/approve`)

        this.checkinId = response.data.checkinId
        this.playerNumber = response.data.playerNumber
        this.questions = response.data.questions
        this.assignedXp = response.data.assignedXp
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
    },
  })
