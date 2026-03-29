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
        this.hasActiveEvent = false
        this.lookupId = ''
        this.eventName = ''
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
            this.activeStepperStep = '3'
            break
          case CheckinStage.EventQuestionsCheck:
            // Questions answered, They need the Stone Puller Next
            this.activeStepperStep = '4'
            break
          case CheckinStage.AssignedXpCheck:
            // Do nothing, this should have automatically
            // Been set SHQ Approval
            break
          case CheckinStage.ShqApproval:
          case CheckinStage.PlayerNeedsReapproval:
            // Stone Pulled, They need to get GO Approval next
            // Redirect them to the character sheet
            if (this.primaryCharacter) {
              await router.push({ name: 'characterSheet', params: { id: this.primaryCharacter.characterId } })
              return
            }
            this.activeStepperStep = '5'
            break
          case CheckinStage.GoApproval:
            // Show need to print CRB

            break
          case CheckinStage.CrbCreation:
            // show that CRB needs to be printed
            this.activeStepperStep = '6'
            break
          case CheckinStage.PrintedCrb:
            // show need to cut and strip CRB
            this.activeStepperStep = '7'
            break
          case CheckinStage.CrbReadForPickup:
            // Show need to verify user pickup
            this.activeStepperStep = '8'
            break
          case CheckinStage.CrbPickedUp:
            // User Has picked up CRB and verified strip info
            if (this.currentEventDay === 1)
              this.activeStepperStep = '9' // Show Friday Finalized
            else
              this.activeStepperStep = '10' // Show Saturday approval
            break
          case CheckinStage.Day2Checkin:
            // User Has picked up CRB and verified strip info
            if (this.currentEventDay === 2)
              this.activeStepperStep = '11' // Show Saturday Finalized
            else
              this.activeStepperStep = '12' // Show Sunday Approval
            break
          case CheckinStage.Day3Checkin:
            // User Has picked up CRB and verified strip info
            this.activeStepperStep = '13' // Show Sunday Finalize
            break
        }
      },
      async verifiedAge(ageTypeId: number, hasWaiver: boolean) {
        await axios.put(`events/checkin/lookup/${this.lookupId}/ageInfo`, {
          ageGroupId: ageTypeId,
          hasSignedConsentForm: hasWaiver,
        })
        await this.resetGoPage()
        this.activeStepperStep = '3'
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
        if (stageId == CheckinStage.CrbPickedUp) {
          await this.verifiedUserInfo()
        }
        else {
          await this.handleStageRedirect(stageId)
        }
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
