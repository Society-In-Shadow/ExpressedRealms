<script setup lang="ts">

import { onBeforeMount, ref } from 'vue'
import { FeatureFlags, userStore } from '@/stores/userStore.ts'
import { EventCheckinStore } from '@/components/conCheckin/stores/eventCheckinStore.ts'
import { useRouter } from 'vue-router'

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

  await eventCheckinInfo.getCheckinInfo()
})

</script>

<template>
  <div>
    <h2>Step 1 - Checkin</h2>
    <ul>
      <li>QR Code</li>
      <li>Find GO</li>
      <li>Find SHQ / Booth</li>
    </ul>
  </div>
  <div>
    <h2>Step 2 - GO Approval</h2>
    <ul>
      <li>Find GO</li>
      <li>They have black vests with a red logo with the following graphic on it: insert photo of logo</li>
    </ul>
  </div>
  <div>
    <h2>Step 3 - Wait For CRB Creation</h2>
    <ul>
      <li>SHQ Needs to create the CRB and get it ready for Play</li>
    </ul>
  </div>
  <div>
    <h2>Step 4 - Pickup CRB</h2>
    <ul>
      <li>Once SHQ has the CRB ready, this message will be updated to say that it's ready to be picked up</li>
      <li>A GO may reach out if they've been notified that your CRB is ready</li>
      <li>You can reach out to SHQ by visiting the Booth</li>
    </ul>
  </div>
</template>
