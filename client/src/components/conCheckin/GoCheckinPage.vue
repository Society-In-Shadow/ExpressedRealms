<script setup lang="ts">

import { onBeforeMount, ref } from 'vue'
import { FeatureFlags, userStore } from '@/stores/userStore.ts'
import { EventCheckinStore } from '@/components/conCheckin/stores/eventCheckinStore.ts'
import { useRouter } from 'vue-router'
import Stepper from 'primevue/stepper'
import StepItem from 'primevue/stepitem'
import Step from 'primevue/step'
import StepPanel from 'primevue/steppanel'
import Button from 'primevue/button'
import StonePuller from '@/components/stonePuller/StonePuller.vue'
import CharacterScanner from '@/components/conCheckin/support/CharacterScanner.vue'

const eventCheckinInfo = EventCheckinStore()
const userInfo = userStore()
const router = useRouter()

const hasCheckinFlag = ref(false)

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
  stepperStep.value = '3'
}

async function onDetect(detectedCodes) {
  eventCheckinInfo.lookupId = detectedCodes
  await eventCheckinInfo.getGoCheckinInfo(detectedCodes)
  stepperStep.value = '2'
}

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
        <Button label="Verified" @click="verifiedPlayerInfo" />
      </StepPanel>
    </StepItem>
    <StepItem value="3">
      <Step>HR Questions</Step>
      <StepPanel>
        <h3>HR Har Harrrr</h3>
      </StepPanel>
    </StepItem>
    <StepItem value="4">
      <Step>Event Questions</Step>
      <StepPanel>
        <h3>Your CRB is ready!</h3>
        <p>Please come find our booth, and pickup up your CRB!</p>
      </StepPanel>
    </StepItem>
    <StepItem value="5">
      <Step>Stone Pull</Step>
      <StepPanel>
        <p>You can use physical stones for this, or use the digital one below.</p>
        <p>Chart is provided as reference for physical stone pull</p>
        <StonePuller :hide-description="true" />
      </StepPanel>
    </StepItem>
    <StepItem value="6">
      <Step>Finalize</Step>
      <StepPanel>
        <h3>Your CRB is ready!</h3>
        <p>Please come find our booth, and pickup up your CRB!</p>
      </StepPanel>
    </StepItem>
  </Stepper>
</template>
