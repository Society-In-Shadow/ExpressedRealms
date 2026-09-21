import { defineStore } from 'pinia'
import axios from 'axios'
import {
  type AgeInfo,
  type ApproveCheckinInfo,
  type AssignedXpType,
  type BasicInfo,
  type CheckinInfo,
  CheckinStage,
  type GetCheckinQuestionsResponse,
  type GetStonePullInfoResponse,
  type GoCheckinInfo,
  type PrimaryCharacterInfo,
  type Question,
} from '@/components/conCheckin/types.ts'
import toaster from '@/services/Toasters'
import router from '@/router'

export const EventCheckinStore
  = defineStore(`eventCheckin`, {
    state: () => {
      return {
        foundInfo: false,
        isReset: false,
        hasActiveEvent: false,
        hasInvalidLookupId: false,
        lookupId: '',
        eventName: '',
        checkinStage: {} as BasicInfo | null,
        goCheckinInfo: {} as GoCheckinInfo,
        playerNumber: 0,
        assignedXp: {} as AssignedXpType | null | undefined,
        primaryCharacter: {} as PrimaryCharacterInfo | null,
        activeStepperStep: '1',
        currentEventDay: 0,
        sendPickupCrbEmail: false,
      }
    },
    actions: {
      async resetGoPage() {
        this.foundInfo = false
        this.isReset = true
        this.lookupId = ''
        this.checkinStage = null
        this.goCheckinInfo = {} as GoCheckinInfo
        this.playerNumber = 0
        this.assignedXp = null
        this.primaryCharacter = null
        this.activeStepperStep = '1'
      },
      async getCheckinAvailable() {
        const response = await axios.get<boolean>(`/events/checkin/available`)
        this.hasActiveEvent = response.data
      },
      async getCheckinInfo() {
        const response = await axios.get<CheckinInfo>(`/events/checkin/info`)

        this.lookupId = response.data.lookupId
        this.eventName = response.data.eventName
        this.checkinStage = response.data.checkinStage
        this.sendPickupCrbEmail = response.data.sendPickupCrbEmail
      },
      async getGoCheckinInfo(lookupId: string): Promise<boolean> {
        const response = await axios.get<GoCheckinInfo>(`/events/checkin/lookup/${encodeURIComponent(lookupId)}`)

        if (!response.data.wasFound) {
          this.hasInvalidLookupId = true
          return false
        }

        this.hasInvalidLookupId = false
        this.goCheckinInfo = response.data
        this.activeStepperStep = '2'
        return true
      },
      async verifiedUserInfo() {
        this.foundInfo = false
        const response = await axios.get<ApproveCheckinInfo>(`/events/checkin/lookup/${this.lookupId}/approve`)

        this.foundInfo = true
        this.playerNumber = response.data.playerNumber
        this.primaryCharacter = response.data.primaryCharacterInfo
        this.checkinStage = response.data.currentStage
        this.isReset = false
        this.currentEventDay = response.data.currentEventDay
        await this.handleStageRedirect(response.data.currentStage.id as CheckinStage)
      },
      async handleStageRedirect(checkinStage: CheckinStage) {
        switch (checkinStage as CheckinStage) {
          default:
            // They need to verify their age
            this.activeStepperStep = '2'
            break
          case CheckinStage.AgeCheckApproval:
            // Age approved, They need to answer event questions next
            this.activeStepperStep = '2'
            break
          case CheckinStage.EventQuestionsCheck:
            // Questions answered, They need the Stone Puller Next
            this.activeStepperStep = '3'
            break
          case CheckinStage.CharacterStorageQuestion:
            this.activeStepperStep = '4'
            break
          case CheckinStage.AssignedXpCheck:
            this.activeStepperStep = '5'
            break
          case CheckinStage.GoApproval:
          case CheckinStage.PlayerNeedsReapproval:
            // Stone Pulled, They need to get GO Approval next
            // Redirect them to the character sheet
            if (this.primaryCharacter) {
              await router.push({ name: 'characterSheet', params: { id: this.primaryCharacter.characterId }, query: { src: 'approve_character' } })
              return
            }
            this.activeStepperStep = '6'
            break
          case CheckinStage.CrbPrinted:
            // show that CRB needs to be printed
            this.activeStepperStep = '8'
            break
          case CheckinStage.CrbAssembled:
            // Show need to verify user pickup
            this.activeStepperStep = '9'
            break
          case CheckinStage.CrbPickedUp:
            this.activeStepperStep = '10'
            break
          case CheckinStage.Day2Checkin:
            if (this.currentEventDay === 1)
              this.activeStepperStep = '10' // Show Friday Finalized
            else
              this.activeStepperStep = '11' // Show Saturday Approval
            break
          case CheckinStage.Day3Checkin:
            // User Has picked up CRB and verified strip info
            if (this.currentEventDay === 2)
              this.activeStepperStep = '12' // Show Saturday Finalized
            else
              this.activeStepperStep = '13' // Show Sunday Approval
            break
          case CheckinStage.FinalStage:
            this.activeStepperStep = '14'
        }
      },
      async verifiedAge(ageTypeId: number, hasWaiver: boolean) {
        await axios.put(`events/checkin/lookup/${this.lookupId}/ageInfo`, {
          ageGroupId: ageTypeId,
          hasSignedConsentForm: hasWaiver,
        })
        this.activeStepperStep = '3'
      },
      async updateCharacterStorage(optedIn: boolean) {
        await axios.put(`events/checkin/lookup/${this.lookupId}/characterStorage`, {
          optedIn: optedIn,
        })
        toaster.success('Character Storage Status Updated!')
        await this.verifiedUserInfo()
      },
      async getVerifiedAge(): Promise<AgeInfo> {
        const response = await axios.get<AgeInfo>(`events/checkin/lookup/${this.lookupId}/ageInfo`)
        return response.data
      },
      async getQuestions(): Promise<GetCheckinQuestionsResponse> {
        const response = await axios.get<GetCheckinQuestionsResponse>(`events/checkin/lookup/${this.lookupId}/questions`)
        return response.data
      },
      async updateQuestion(question: Question) {
        await axios.put(`/events/checkin/lookup/${this.lookupId}/questions/${question.id}`, { response: question.response })
        toaster.success('Question updated successfully!')
      },
      async getStonePullInformation(): Promise<GetStonePullInfoResponse> {
        const response = await axios.get<GetStonePullInfoResponse>(`/events/checkin/lookup/${this.lookupId}/assignXp`)
        return response.data
      },
      async addAssignedXp(typeId: number, amount: number) {
        await axios.post(`/events/checkin/lookup/${this.lookupId}/assignXp`, { amount: amount, AssignedXpTypeId: typeId })

        toaster.success('Assigned XP successfully!')
        await this.handleStageRedirect(CheckinStage.ShqApproval)
      },
      async approveStage(stageId: number) {
        await axios.post(`/events/checkin/lookup/${this.lookupId}/approveStage`, { stageId: stageId })
        await this.verifiedUserInfo()
        toaster.success('Stage approved successfully!')
      },
      async approveCharacterSheet() {
        await axios.post(`/events/checkin/lookup/${this.lookupId}/approveStage`, { stageId: CheckinStage.GoApproval })
        toaster.success('Character Sheet Approval successfully!')
        await this.resetGoPage()

        await router.push({ name: 'gocheckin' })
      },
      async retireCharacter() {
        await axios.put(`/characters/${this.lookupId}/retire`)
        await this.resetGoPage()
      },
      async updateCrbEmailFlag() {
        await axios.put(`/events/checkin/updateCrbEmail`, { enableCrbEmailNotification: this.sendPickupCrbEmail })
        toaster.success('Notification Preference Updated!')
        await this.resetGoPage()
      },
    },
  })
