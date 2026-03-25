<script setup lang="ts">

import Button from 'primevue/button'
import Checkbox from 'primevue/checkbox'
import { computed, onBeforeMount, ref, watch } from 'vue'
import { EventCheckinStore } from '@/components/conCheckin/stores/eventCheckinStore.ts'
import { confirmationPopups } from '@/components/conCheckin/services/popupService.ts'
import { AgeGroupId, type AgeInfo } from '@/components/conCheckin/types.ts'

const eventCheckinInfo = EventCheckinStore()
const popups = confirmationPopups()

const is13OrOlder = ref(false)
const is18OrOlder = ref(false)
const signedWaiver = ref(false)
const ageInfo = ref<AgeInfo | null>(null)

onBeforeMount(async () => {
  ageInfo.value = await eventCheckinInfo.getVerifiedAge()
  if (ageInfo.value!.ageGroupId == AgeGroupId.Adult) {
    is13OrOlder.value = true
    is18OrOlder.value = true
  }
  else if (ageInfo.value!.ageGroupId == AgeGroupId.Teen) {
    is13OrOlder.value = true
    signedWaiver.value = true
  }
})

watch(() => eventCheckinInfo.isReset, (old, newValue) => {
  if (eventCheckinInfo.isReset) {
    ageInfo.value = null
  }
})

async function verifiedPlayerInfo() {
  console.log('this should not be hit')
  if (signedWaiver.value) {
    await eventCheckinInfo.verifiedAge(AgeGroupId.Teen, true) // Teen Group, with signed waiver
  }
  else if (is18OrOlder.value) {
    await eventCheckinInfo.verifiedAge(AgeGroupId.Adult, false) // Adult group, no need for waiver
  }
}

const verifiedMessage = computed(() => {
  if (ageInfo.value.ageGroupId == AgeGroupId.Teen) {
    return 'a Teen (13-17) and they have a signed waiver'
  }
  return 'an adult (18+)'
})

</script>

<template>
  <h2>{{ eventCheckinInfo.goCheckinInfo.userName }}</h2>
  <div v-if="ageInfo == null">
    Loading...
  </div>
  <div v-else-if="!ageInfo.hasBeenVerified">
    <h2 v-if="ageInfo && ageInfo.ageGroupId == null">
      Looks like this is your first time playing!
    </h2>
    <h2 v-if="signedWaiver && !ageInfo.ageGroupId == null">
      They need to reverify their age, they are in the teen bracket
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
    <Button severity="secondary" label="They are under 13" class="w-100" @click="popups.childConfirmation($event)" />
    <Button label="Verified" class="w-100 mt-3 mb-3" :disabled="!is13OrOlder || !is18OrOlder && !signedWaiver || ageInfo.hasBeenVerified" @click="verifiedPlayerInfo" />
  </div>
  <div v-else>
    <p>User age has been verified as {{ verifiedMessage }} for this con.</p>
    <p>If this is incorrect, please contact SHQ.</p>
    <Button label="Verfied Info" @click="eventCheckinInfo.verifiedUserInfo()" />
  </div>
</template>
