<script setup lang="ts">

import { onBeforeMount, ref, watch } from 'vue'
import { FeatureFlags, userStore } from '@/stores/userStore.ts'
import { EventCheckinStore } from '@/components/conCheckin/stores/eventCheckinStore.ts'
import { useRouter } from 'vue-router'
import Stepper from 'primevue/stepper'
import StepItem from 'primevue/stepitem'
import Step from 'primevue/step'
import StepPanel from 'primevue/steppanel'
import Button from 'primevue/button'
import CharacterScanner from '@/components/conCheckin/support/CharacterScanner.vue'
import AnswerQuestions from '@/components/conCheckin/support/AnswerQuestions.vue'
import StonePullerStep from '@/components/conCheckin/support/StonePullerStep.vue'
import Message from 'primevue/message'
import { userPermissionStore } from '@/stores/userPermissionStore.ts'
import { confirmationPopups } from '@/components/conCheckin/services/popupService.ts'
import AgeVerificationStep from '@/components/conCheckin/support/AgeVerificationStep.vue'
import DailyCheckin from '@/components/conCheckin/support/DailyCheckin.vue'

const eventCheckinInfo = EventCheckinStore()
const userPermission = userPermissionStore()
const userInfo = userStore()
const router = useRouter()
const popups = confirmationPopups()

const permissionCheck = userPermission.permissionCheck
const hasCheckinFlag = ref(false)

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
    stepperStep.value = '1'
  }
})

const stepperStep = ref('1')

async function onDetect(detectedCodes) {
  eventCheckinInfo.lookupId = detectedCodes
  await eventCheckinInfo.getGoCheckinInfo(detectedCodes)
}

const approveStage = async (stageId: number) => {
  await eventCheckinInfo.approveStage(stageId)
}

</script>

<template>
  <Message v-if="eventCheckinInfo.isReset" severity="success">
    Successfully committed Stage!
  </Message>
  <div v-if="permissionCheck.CharacterManagement.Retire" class="text-right">
    <Button label="Retire Character" @click="popups.retireConfirmation($event, eventCheckinInfo.primaryCharacter.characterName)" />
  </div>
  <Stepper v-model:value="eventCheckinInfo.activeStepperStep">
    <StepItem value="1">
      <Step>Scan QR Code</Step>
      <StepPanel v-if="eventCheckinInfo.activeStepperStep == '1'">
        <p>Ask the user to open up their account, and click on the "Event Check-in" tile at the top of the page</p>
        <p>This should pull up a QR code you can scan below</p>
        <CharacterScanner @detected-code="onDetect" />
      </StepPanel>
    </StepItem>
    <StepItem value="2">
      <Step>Verify User Info</Step>
      <StepPanel v-if="eventCheckinInfo.activeStepperStep == '2'">
        <AgeVerificationStep />
      </StepPanel>
    </StepItem>
    <StepItem value="3">
      <Step>HR Questions</Step>
      <StepPanel v-if="eventCheckinInfo.activeStepperStep == '3'">
        <AnswerQuestions />
      </StepPanel>
    </StepItem>
    <StepItem value="4">
      <Step>Stone Pull</Step>
      <StepPanel v-if="eventCheckinInfo.activeStepperStep == '4'">
        <StonePullerStep />
      </StepPanel>
    </StepItem>
    <StepItem value="5" :disabled="stepperStep !== '5'">
      <Step>GO Approval</Step>
      <StepPanel>
        <h3>Link to their CRB</h3>
        <p v-if="!eventCheckinInfo.primaryCharacter">
          They do not have a primary character setup yet, you will need to walk them through how to do that.  You will need to recheck them in after they have one
        </p>
      </StepPanel>
    </StepItem>
    <StepItem value="6">
      <Step>CRB Needs Printing</Step>
      <StepPanel>
        <h3>The CRB needs to be printed</h3>
        <p>SHQ needs to print the CRB.</p>
        <p>This will automatically update once it's been printed at least once</p>
      </StepPanel>
    </StepItem>
    <StepItem value="7">
      <Step>CRB Needs Creation</Step>
      <StepPanel>
        <h3>CRB needs to be created</h3>
        <p>CRB was printed, need to cut and strip it.  Once done, scan it and click the below button</p>
        <Button
          label="CRB Created and Ready for Pickup" icon-pos="right" class="mb-4" :disabled="!permissionCheck.Event.CrbHandling"
          @click="approveStage(4)"
        />
      </StepPanel>
    </StepItem>
    <StepItem value="8">
      <Step>CRB Is Ready for Pickup</Step>
      <StepPanel>
        <h3>Needs to be Verified by User</h3>
        <p>User needs to check their CRB, and make sure it's good to go.  Once scanned, they can go play games</p>
        <Button
          label="CRB Is Picked Up" icon-pos="right" class="mb-4" :disabled="!permissionCheck.Event.CrbHandling"
          @click="approveStage(5)"
        />
      </StepPanel>
    </StepItem>
    <StepItem value="9">
      <Step>Picked Up and Day One</Step>
      <StepPanel v-if="eventCheckinInfo.activeStepperStep == '9'">
        <h3>Everything is Done for Day 1</h3>
        <DailyCheckin />
        <Button
          label="Day 2 Check-in" icon-pos="right" class="mb-4" :disabled="!permissionCheck.Event.Day23Checkin"
          @click="approveStage(6)"
        />
      </StepPanel>
    </StepItem>
    <StepItem value="10">
      <Step>Day 2 Checkin</Step>
      <StepPanel>
        <h3>This is the 2nd day the character has been in play, not the 2nd day of play</h3>
        <p>The character has been checked in for the day.</p>
        <Button
          label="Day 3 Check-in" icon-pos="right" class="mb-4" :disabled="!permissionCheck.Event.Day23Checkin"
          @click="approveStage(7)"
        />
      </StepPanel>
    </StepItem>
    <StepItem value="11">
      <Step>Day 3</Step>
      <StepPanel>
        <h3>Thank You and Come Again!</h3>
        <p>The character has been checked in for the day.</p>
        <p>Everything is Done for the Con</p>
      </StepPanel>
    </StepItem>
  </Stepper>
</template>
