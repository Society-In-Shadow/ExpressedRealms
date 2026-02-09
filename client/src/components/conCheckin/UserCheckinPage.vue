<script setup lang="ts">

import { onBeforeMount, ref, shallowRef } from 'vue'
import { FeatureFlags, userStore } from '@/stores/userStore.ts'
import { EventCheckinStore } from '@/components/conCheckin/stores/eventCheckinStore.ts'
import { useRouter } from 'vue-router'
import { useQRCode } from '@vueuse/integrations/useQRCode'
import Stepper from 'primevue/stepper'
import StepItem from 'primevue/stepitem'
import Step from 'primevue/step'
import StepPanel from 'primevue/steppanel'

const eventCheckinInfo = EventCheckinStore()
const userInfo = userStore()
const router = useRouter()

const hasCheckinFlag = ref(false)
const text = shallowRef('')
let qrcode = useQRCode(text, {
  errorCorrectionLevel: 'H',
  margin: 5,
})

onBeforeMount(async () => {
  await eventCheckinInfo.getCheckinAvailable()
  hasCheckinFlag.value = await userInfo.hasFeatureFlag(FeatureFlags.ShowEventCheckin)

  if (!hasCheckinFlag.value || !eventCheckinInfo.hasActiveEvent) {
    await router.push({ name: 'characters' })
  }

  await eventCheckinInfo.getCheckinInfo()
  text.value = eventCheckinInfo.lookupId
})

</script>

<template>
  <Stepper value="1">
    <StepItem value="1">
      <Step>Initial Checkin</Step>
      <StepPanel>
        <h3>Welcome to Event Name and Society in Shadows!</h3>
        <p>Before we get started, lets define a few terms you will commonly hear</p>
        <ul>
          <li>GO - Game Official - These are the people that will run games, and help you with character creation.</li>
          <li>SHQ - Staff Head Quarters - This is our booth that we have setup at the con.</li>
          <li>CRB - Character Reference Booklet - This is your character sheet, it's what you'll use to play the game.</li>
        </ul>
        <p>With that out of the way, please present the QR Code below to a GO or SHQ to get started!</p>
        <div class="d-flex flex-column align-items-start">
          <div class="text-center w-md-100">
            <img :src="qrcode" alt="QR Code" class="w-md-100" style="min-width: 250px">
            <div>{{ text }}</div>
          </div>
        </div>
      </StepPanel>
    </StepItem>
    <StepItem value="2">
      <Step>GO Approval</Step>
      <StepPanel>
        <h3>On the hunt for a GO</h3>
        <p>A GO is required to review your character sheet, they will ensure that your character is ready to play the game.</p>
        <p>
          They will also fill you in with any relevant information regarding the game, any history you might need to know,
          and any other information they deem necessary.
        </p>
        <p>This is also where you want to confirm your characters background, and any other questions you might have.</p>
        <p>SHQ can assist you in finding a GO if you are having trouble finding one.</p>
        <p>They have black vests with a red logo with the following graphic on it: insert photo of logo</p>
        <img src="/public/favicon.png" alt="Society in Shadows vest emblem">
      </StepPanel>
    </StepItem>
    <StepItem value="3">
      <Step>CRB is Cooking!</Step>
      <StepPanel>
        <h3>The CRB is cooking!</h3>
        <p>SHQ needs time to pull together your CRB</p>
        <p>A CRB is your Character Reference Booklet, it's what you'll use to play the game.</p>
        <p>It's provided to you for free, only thing we ask is that you return all of your strips at the end of the game.</p>
      </StepPanel>
    </StepItem>
    <StepItem value="4">
      <Step>CRB Ready!</Step>
      <StepPanel>
        <h3>Your CRB is ready!</h3>
        <p>Please come find our booth, and pickup up your CRB!</p>
      </StepPanel>
    </StepItem>
  </Stepper>
</template>

<style>
@media(max-width: 768px){
  .w-md-100{
    width: 100% !important;
  }
}
</style>
