<script setup lang="ts">

import Message from 'primevue/message'
import Button from 'primevue/button'
import { useRouter } from 'vue-router'

const router = useRouter()

async function redirectToCheckinDetails() {
  await router.push({ name: 'eventcheckin' })
}

</script>

<template>
  <Message class="sticky-top ms-0 me-0 mt-2 mb-2 m-md-2 d-print-none custom-message" severity="info">
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
