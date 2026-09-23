<script setup lang="ts">

import Button from 'primevue/button'
import { useRouter } from 'vue-router'
import { onBeforeMount } from 'vue'
import { EventCheckinStore } from '@/components/conCheckin/stores/eventCheckinStore.ts'

const router = useRouter()
const eventCheckinInfo = EventCheckinStore()

onBeforeMount(async () => {
  await eventCheckinInfo.getCheckinAvailable()
})

async function redirectToCheckinDetails() {
  await router.push({ name: 'eventcheckin' })
}

</script>

<template>
  <div v-if="eventCheckinInfo.hasActiveEvent" class="custom-message m-1 m-md-3 pl-3 pr-3 pt-2 pb-2" @click="redirectToCheckinDetails">
    <div class="d-flex align-items-center">
      <h3 class="m-0 p-0 flex-fill">
        Event Check-in
      </h3>
      <Button label="Checkin" size="small" @click="redirectToCheckinDetails" />
    </div>
  </div>
</template>

<style>

.custom-message {
  cursor: pointer;
}

</style>
