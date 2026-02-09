<script setup lang="ts">

import { nextTick, onBeforeMount, ref } from 'vue'
import { FeatureFlags, userStore } from '@/stores/userStore.ts'
import { EventCheckinStore } from '@/components/conCheckin/stores/eventCheckinStore.ts'
import { useRouter } from 'vue-router'
import Stepper from 'primevue/stepper'
import StepItem from 'primevue/stepitem'
import Step from 'primevue/step'
import StepPanel from 'primevue/steppanel'
import { QrcodeStream } from 'vue-qrcode-reader'
import Button from 'primevue/button'
import InputText from 'primevue/inputtext'
import StonePuller from '@/components/stonePuller/StonePuller.vue'

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

const loading = ref(true)
const destroyed = ref(false)
const result = ref('')
const stepperStep = ref('1')

function onCameraOn() {
  loading.value = false
}

async function reload() {
  destroyed.value = true
  await nextTick()
  destroyed.value = false
  loading.value = true
}

async function onDetect(detectedCodes) {
  result.value = detectedCodes.map(code => code.rawValue)[0]
  await eventCheckinInfo.getGoCheckinInfo(result.value)
  stepperStep.value = '2'
}

async function lookupManual() {
  await eventCheckinInfo.getGoCheckinInfo(result.value)
  stepperStep.value = '2'
}

function clearResult() {
  result.value = ''
}

function paintOutline(detectedCodes, ctx) {
  for (const detectedCode of detectedCodes) {
    const [firstPoint, ...otherPoints] = detectedCode.cornerPoints

    ctx.strokeStyle = 'red'

    ctx.beginPath()
    ctx.moveTo(firstPoint.x, firstPoint.y)
    for (const { x, y } of otherPoints) {
      ctx.lineTo(x, y)
    }
    ctx.lineTo(firstPoint.x, firstPoint.y)
    ctx.closePath()
    ctx.stroke()
  }
}

</script>

<template>
  <Stepper v-model:value="stepperStep">
    <StepItem value="1">
      <Step>Scan QR Code</Step>
      <StepPanel>
        <p>Ask the user to open up their account, and click on the "Event Checkin Details" tile at the top of the page</p>
        <p>This should pull up a QR code you can scan below</p>
        <div class="w-100">
          <qrcode-stream
            v-if="!destroyed && result == ''"
            :track="paintOutline"
            @camera-on="onCameraOn"
            @detect="onDetect"
          >
            <div
              v-if="loading"
              class="loading-indicator"
            >
              Loading...
            </div>
          </qrcode-stream>
        </div>
        <div>
          <p>If the scanner is not working, you can manually put in the lookup below</p>
          <InputText v-model="result" placeholder="Manual Lookup" minlength="8" maxlength="8" />
          <Button label="Lookup" @click="lookupManual" />
        </div>
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
        <Button label="Verified" @click="activateCallback('3')" />
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
        <StonePuller />
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

<style>
@media(max-width: 768px){
  .w-md-100{
    width: 100% !important;
  }
}
</style>
