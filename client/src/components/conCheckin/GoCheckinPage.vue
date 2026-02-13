<script setup lang="ts">

import { computed, onBeforeMount, ref } from 'vue'
import { FeatureFlags, userStore } from '@/stores/userStore.ts'
import { EventCheckinStore } from '@/components/conCheckin/stores/eventCheckinStore.ts'
import { useRouter } from 'vue-router'
import Stepper from 'primevue/stepper'
import StepItem from 'primevue/stepitem'
import Step from 'primevue/step'
import StepPanel from 'primevue/steppanel'
import Button from 'primevue/button'
import Checkbox from 'primevue/checkbox'
import StonePuller from '@/components/stonePuller/StonePuller.vue'
import CharacterScanner from '@/components/conCheckin/support/CharacterScanner.vue'
import AnswerQuestions from '@/components/conCheckin/support/AnswerQuestions.vue'
import type { Question } from '@/components/conCheckin/types.ts'

const eventCheckinInfo = EventCheckinStore()
const userInfo = userStore()
const router = useRouter()

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
  stepperStep.value = '2'
}

const waiverStatus = computed(() => {
  if (signedWaiver.value) return 'Under 18 - Signed Waiver'
  return 'Over 18'
})

</script>

<template>
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
          <label id="13AgeQuestion">Are you 13 years or older?</label>
        </div>
        <div class="d-flex self-align-center gap-2 mb-3">
          <Checkbox id="18AgeQuestion" v-model="is18OrOlder" binary :disabled="signedWaiver" @change="is13OrOlder = true" />
          <label id="18AgeQuestion">Are you 18 years or older?</label>
        </div>
        <div class="d-flex self-align-center gap-2 mb-3">
          <Checkbox id="signedwaiver" v-model="signedWaiver" binary :disabled="is18OrOlder" @change="is13OrOlder = true" />
          <label id="signedwaiver">If not, have you signed a waiver? (Front Desk will have these)</label>
        </div>
        <p>If they fall into above category, send them to the front desk to get this resolved.</p>
        <Button label="Verified" :disabled="!is13OrOlder || !is18OrOlder && !signedWaiver" @click="verifiedPlayerInfo" />
      </StepPanel>
    </StepItem>
    <StepItem value="3">
      <Step>HR Questions</Step>
      <StepPanel>
        <AnswerQuestions />
      </StepPanel>
    </StepItem>
    <StepItem value="4">
      <Step>Stone Pull</Step>
      <StepPanel>
        <p>You can use physical stones for this, or use the digital one below.</p>
        <p>Chart is provided as reference for physical stone pull</p>
        <StonePuller :hide-description="true" />
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
      </StepPanel>
    </StepItem>
    <StepItem value="6" :disabled="stepperStep !== '6'">
      <Step>GO Approval</Step>
      <StepPanel>
        <h3>Link to their CRB</h3>
        <p>I'm a Link!</p>
        <h3>They do not have a primary character, you will need to walk them through how to do that</h3>
        <h3>Did you approve the contacts on their CRB? (Block till they say yes)</h3>
        <h3>Did you Check to make sure that most of their XP has been spent? (Eg, they haven't spent anything outside of character creation)</h3>
        <h3>Is their character level within expections for the plot?</h3>
      </StepPanel>
    </StepItem>
    <StepItem value="7">
      <Step>CRB Creation</Step>
      <StepPanel>
        <h3>CRB needs to be created</h3>
      </StepPanel>
    </StepItem>
    <StepItem value="8">
      <Step>CRB Is Ready for Pickup</Step>
      <StepPanel>
        <h3>Need to scan to move to next step</h3>
      </StepPanel>
    </StepItem>
    <StepItem value="9">
      <Step>CRB Is Picked Up</Step>
      <StepPanel>
        <h3>Needs to be scanned again to be verified by user</h3>
      </StepPanel>
    </StepItem>
  </Stepper>
</template>
