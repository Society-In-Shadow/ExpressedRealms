import { defineStore } from 'pinia'
import axios from 'axios'
import type { ApproveCheckinInfo, CheckinInfo, GoCheckinInfo, Question } from '@/components/conCheckin/types.ts'

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
      },
      async updateQuestion(question: Question) {
        await axios.put(`/events/checkin/lookup/${this.lookupId}/questions/${question.id}`, { response: question.response })
      },
    },
  })
