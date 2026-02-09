<script setup lang="ts">

import Message from 'primevue/message'
import Button from 'primevue/button'
import { useRouter } from 'vue-router'
import { computed, onBeforeMount, ref } from 'vue'
import { FeatureFlags, userStore } from '@/stores/userStore.ts'
import { EventCheckinStore } from '@/components/conCheckin/stores/eventCheckinStore.ts'

const router = useRouter()
const userInfo = userStore()
const eventCheckinInfo = EventCheckinStore()

const hasCheckinFlag = ref(false)

onBeforeMount(async () => {
  await eventCheckinInfo.getCheckinAvailable()
  hasCheckinFlag.value = await userInfo.hasFeatureFlag(FeatureFlags.ShowEventCheckin)
})

const showBanner = computed(() => hasCheckinFlag.value && eventCheckinInfo.hasActiveEvent)

async function redirectToCheckinDetails() {
  await router.push({ name: 'eventcheckin' })
}

</script>

<template>
  <Message v-if="showBanner" class="ms-0 me-0 mt-2 mb-2 m-md-2 d-print-none custom-message" severity="info">
    <template #icon>
      <span class="material-symbols-outlined">
        local_activity
      </span>
    </template>
    <div class="d-flex flex-row justify-content-between align-self-center align-items-center">
      <div class="flex-fill">
        Event Checkin Details
      </div>
      <div><Button label="Checkin" size="small" @click="redirectToCheckinDetails" /></div>
    </div>
  </Message>
</template>

<style>

.custom-message .p-message-text {
  width: 100%;
}

</style>
