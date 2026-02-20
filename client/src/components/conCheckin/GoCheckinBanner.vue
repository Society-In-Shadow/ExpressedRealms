<script setup lang="ts">

import Message from 'primevue/message'
import Button from 'primevue/button'
import { useRouter } from 'vue-router'
import { computed, onBeforeMount, ref } from 'vue'
import { EventCheckinStore } from '@/components/conCheckin/stores/eventCheckinStore.ts'
import { userPermissionStore } from '@/stores/userPermissionStore.ts'

const router = useRouter()
const permissionInfo = userPermissionStore()
const permissionCheck = permissionInfo.permissionCheck

const eventCheckinInfo = EventCheckinStore()

const hasCheckinPermission = ref(false)

onBeforeMount(async () => {
  await eventCheckinInfo.getCheckinAvailable()
  hasCheckinPermission.value = permissionCheck.Event.Checkin
})

const showBanner = computed(() => eventCheckinInfo.hasActiveEvent && hasCheckinPermission.value)

async function redirectToCheckinDetails() {
  await router.push({ name: 'gocheckin' })
}

</script>

<template>
  <Message v-if="showBanner" class="ms-0 me-0 mt-2 mb-2 m-md-2 d-print-none custom-message" severity="info" @click="redirectToCheckinDetails">
    <template #icon>
      <span class="material-symbols-outlined">
        confirmation_number
      </span>
    </template>
    <div class="d-flex flex-row justify-content-between align-self-center align-items-center">
      <div class="flex-fill">
        Check-in User
      </div>
      <div><Button label="Checkin" size="small" @click="redirectToCheckinDetails" /></div>
    </div>
  </Message>
</template>

<style>

.custom-message .p-message-text {
  width: 100%;
}

.custom-message {
  cursor: pointer;
}

</style>
