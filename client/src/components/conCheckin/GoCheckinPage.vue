<script setup lang="ts">

import { computed, onBeforeMount, ref, watch } from 'vue'
import { FeatureFlags, userStore } from '@/stores/userStore.ts'
import { EventCheckinStore } from '@/components/conCheckin/stores/eventCheckinStore.ts'
import { useRouter } from 'vue-router'
import Stepper from 'primevue/stepper'
import StepItem from 'primevue/stepitem'
import Step from 'primevue/step'
import StepPanel from 'primevue/steppanel'
import Button from 'primevue/button'
import Checkbox from 'primevue/checkbox'
import CharacterScanner from '@/components/conCheckin/support/CharacterScanner.vue'
import AnswerQuestions from '@/components/conCheckin/support/AnswerQuestions.vue'
import type { Question } from '@/components/conCheckin/types.ts'
import StonePullerStep from '@/components/conCheckin/support/StonePullerStep.vue'
import Message from 'primevue/message'
import { userPermissionStore } from '@/stores/userPermissionStore.ts'

const eventCheckinInfo = EventCheckinStore()
const userPermission = userPermissionStore()
const userInfo = userStore()
const router = useRouter()

const permissionCheck = userPermission.permissionCheck
const hasCheckinFlag = ref(false)
const is13OrOlder = ref(false)
const is18OrOlder = ref(false)
const signedWaiver = ref(false)

onBeforeMount(async () => {
  await eventCheckinInfo.getCheckinAvailable()
  hasCheckinFlag.value = await userInfo.hasFeatureFlag(FeatureFlags.ShowEventCheckin)

  if (!hasCheckinFlag.value || !eventCheckinInfo.hasActiveEvent) {
    await router.push({ name: 'characters' })
  }
})

watch(() => eventCheckinInfo.isReset, (old, newValue) => {
  if (eventCheckinInfo.isReset) {
    hasCheckinFlag.value = false
    is13OrOlder.value = false
    is18OrOlder.value = false
    signedWaiver.value = false
    stepperStep.value = '1'
  }
})

const stepperStep = ref('1')

async function verifiedPlayerInfo() {
  await eventCheckinInfo.verifiedUserInfo()
  const waiverQuestion = eventCheckinInfo.questions.find((x: Question) => x.typeId == 1)
  waiverQuestion!.response = waiverStatus
  await eventCheckinInfo.updateQuestion(waiverQuestion!)

  stepperStep.value = '3'
}

async function onDetect(detectedCodes) {
  eventCheckinInfo.lookupId = detectedCodes
  await eventCheckinInfo.getGoCheckinInfo(detectedCodes)

  if (eventCheckinInfo.goCheckinInfo.alreadyCheckedIn) {
    await eventCheckinInfo.verifiedUserInfo()
    const waiverQuestion = eventCheckinInfo.questions.find((x: Question) => x.typeId == 1)
    if (waiverQuestion?.response == 'Over 18') {
      is13OrOlder.value = true
      is18OrOlder.value = true
    }
    else if (waiverQuestion?.response == 'Under 18 - Signed Waiver') {
      is13OrOlder.value = true
      signedWaiver.value = true
    }
  }

  const stageId = eventCheckinInfo.checkinStage?.id
  if (stageId && stageId >= 1) {
    stepperStep.value = String(stageId + 5)
  }
  else {
    stepperStep.value = '2'
  }
}

const canFinalizeStage = (stageId: number) => {
  if (eventCheckinInfo.checkinStage == null && stageId == 5)
    return false
  return eventCheckinInfo.checkinStage?.id + 5 != stageId
}

const isFinalized = (stageId: number) => {
  if (eventCheckinInfo.checkinStage?.id + 5 > stageId)
    return 'pi pi-check'
  return ''
}

const waiverStatus = computed(() => {
  if (signedWaiver.value) return 'Under 18 - Signed Waiver'
  return 'Over 18'
})

const approveStage = async (stageId: number) => {
  await eventCheckinInfo.approveStage(stageId)
}

const typeName = (typeId: number) => {
  switch (typeId) {
    case 2:
      return 'Checkin Bonus'
    case 5:
      return 'Brought New Player'
    case 4:
      return 'First Time Player'
    default:
      return 'Unknown'
  }
}

</script>

<template>
  <Message v-if="eventCheckinInfo.isReset" severity="success">
    Successfully committed Stage!
  </Message>
  <Stepper v-model:value="stepperStep">
    <StepItem value="1">
      <Step>Scan QR Code</Step>
      <StepPanel>
        <p>Ask the user to open up their account, and click on the "Event Checkin Details" tile at the top of the page</p>
        <p>This should pull up a QR code you can scan below</p>
        <CharacterScanner @detected-code="onDetect" />
      </StepPanel>
    </StepItem>
    <StepItem value="2">
      <Step>Verify User Info</Step>
      <StepPanel v-slot="{activateCallback}">
        <h1>Character Scanned!</h1>
        <h2>Is your name {{ eventCheckinInfo.goCheckinInfo.userName }}?</h2>
        <h2 v-if="eventCheckinInfo.goCheckinInfo.isFirstTimeUser">
          Looks like this is your first time playing!
        </h2>
        <div class="d-flex self-align-center gap-2 mb-3">
          <Checkbox id="13AgeQuestion" v-model="is13OrOlder" binary />
          <label for="13AgeQuestion">Are you 13 years or older?</label>
        </div>
        <div class="d-flex self-align-center gap-2 mb-3">
          <Checkbox id="18AgeQuestion" v-model="is18OrOlder" binary :disabled="signedWaiver" @change="is13OrOlder = true" />
          <label for="18AgeQuestion">Are you 18 years or older?</label>
        </div>
        <div class="d-flex self-align-center gap-2 mb-3">
          <Checkbox id="signedwaiver" v-model="signedWaiver" binary :disabled="is18OrOlder" @change="is13OrOlder = true" />
          <label for="signedwaiver">If not, have you signed a waiver? (Front Desk will have these)</label>
        </div>
        <p>If they fall into above category, send them to the front desk to get this resolved.</p>
        <Button label="Verified" :disabled="!is13OrOlder || !is18OrOlder && !signedWaiver || eventCheckinInfo.goCheckinInfo.alreadyCheckedIn" @click="verifiedPlayerInfo" />
        <Button label="Reviewed" icon="pi pi-arrow-right" icon-pos="right" class="mb-4 ml-3" @click="activateCallback('3')" />
      </StepPanel>
    </StepItem>
    <StepItem value="3">
      <Step>HR Questions</Step>
      <StepPanel v-slot="{activateCallback}">
        <AnswerQuestions />
        <Button label="Reviewed" icon="pi pi-arrow-right" icon-pos="right" class="mb-4" @click="activateCallback('4')" />
      </StepPanel>
    </StepItem>
    <StepItem value="4">
      <Step>Stone Pull</Step>
      <StepPanel v-slot="{activateCallback}">
        <StonePullerStep v-if="stepperStep == '4'" />
        <Button label="Reviewed" icon="pi pi-arrow-right" icon-pos="right" class="mb-4" @click="activateCallback('5')" />
      </StepPanel>
    </StepItem>
    <StepItem value="5">
      <Step>Review and Finalize Initial Checkin</Step>
      <StepPanel>
        <h1>Review</h1>
        <h2>Questions</h2>
        <div v-for="question in eventCheckinInfo.questions" :key="question.id">
          <h3>{{ question.question }}</h3>
          <p>{{ question.response }}</p>
        </div>
        <h2>Checkin Bonus</h2>
        <p>+{{ eventCheckinInfo.assignedXp?.amount }} - {{ typeName(eventCheckinInfo.assignedXp?.typeId) }}</p>
        <Button
          label="Finalize Checkin" :icon="isFinalized(5)" icon-pos="right" class="mb-4" :disabled="canFinalizeStage(5)"
          @click="approveStage(1)"
        />
      </StepPanel>
    </StepItem>
    <StepItem value="6" :disabled="stepperStep !== '6'">
      <Step>GO Approval</Step>
      <StepPanel>
        <h3>Link to their CRB</h3>
        <p v-if="!eventCheckinInfo.primaryCharacter">
          They do not have a primary character setup yet, you will need to walk them through how to do that.  Approving has been disabled until they have one.  You will need to recheck them in after they have one
        </p>
        <p v-else>
          <RouterLink :to="`/characters/${eventCheckinInfo.primaryCharacter.characterId}`" target="_blank">
            {{ eventCheckinInfo.primaryCharacter.characterName }}
          </RouterLink>
        </p>
        <h3>Did you approve the contacts on their CRB? (Block till they say yes)</h3>
        <h3>Did you Check to make sure that most of their XP has been spent? (Eg, they've spent xp outside of character creation)</h3>
        <h3>Is their character level within expections for the plot?</h3>
        <Button
          label="GO Approval" :icon="isFinalized(6)" icon-pos="right" class="mb-4" :disabled="canFinalizeStage(6) || !permissionCheck.Event.GoApproval || !eventCheckinInfo.primaryCharacter"
          @click="approveStage(2)"
        />
      </StepPanel>
    </StepItem>
    <StepItem value="8">
      <Step>CRB Creation</Step>
      <StepPanel>
        <h3>CRB needs to be created</h3>
        <p>SHQ needs to create the CRB.  Once ready, scan it and click the below button</p>
        <Button
          label="CRB Created and Ready for Pickup" :icon="isFinalized(8)" icon-pos="right" class="mb-4" :disabled="canFinalizeStage(8) || !permissionCheck.Event.CrbHandling"
          @click="approveStage(4)"
        />
      </StepPanel>
    </StepItem>
    <StepItem value="9">
      <Step>CRB Is Ready for Pickup</Step>
      <StepPanel>
        <h3>Needs to be Verified by User</h3>
        <p>User needs to check their CRB, and make sure it's good to go.  Once scanned, they can go play games</p>
        <Button
          label="CRB Is Picked Up" :icon="isFinalized(9)" icon-pos="right" class="mb-4" :disabled="canFinalizeStage(9) || !permissionCheck.Event.CrbHandling"
          @click="approveStage(5)"
        />
      </StepPanel>
    </StepItem>
    <StepItem value="10">
      <Step>Picked Up and Day One</Step>
      <StepPanel>
        <h3>Everything is Done for Day 1</h3>
        <Button
          label="Day 2 Checkin" :icon="isFinalized(10)" icon-pos="right" class="mb-4" :disabled="canFinalizeStage(10) || !permissionCheck.Event.Day23Checkin"
          @click="approveStage(6)"
        />
      </StepPanel>
    </StepItem>
    <StepItem value="11">
      <Step>Day 2 Checkin</Step>
      <StepPanel>
        <h3>This is the 2nd day the character has been in play, not the 2nd day of play</h3>
        <p>The character has been checked in for the day.</p>
        <Button
          label="Day 3 Checkin" :icon="isFinalized(11)" icon-pos="right" class="mb-4" :disabled="canFinalizeStage(11) || !permissionCheck.Event.Day23Checkin"
          @click="approveStage(7)"
        />
      </StepPanel>
    </StepItem>
    <StepItem value="12">
      <Step>Day 3</Step>
      <StepPanel>
        <h3>Thank You and Come Again!</h3>
        <p>The character has been checked in for the day.</p>
        <p>Everything is Done for the Con</p>
      </StepPanel>
    </StepItem>
  </Stepper>
</template>
