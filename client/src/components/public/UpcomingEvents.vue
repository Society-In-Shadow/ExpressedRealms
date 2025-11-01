<script setup lang="ts">
import Card from 'primevue/card'

import { onBeforeMount } from 'vue'
import { EventStore } from '@/components/admin/events/stores/eventStore.ts'
import EventList from '@/components/admin/events/EventList.vue'

const store = EventStore()

onBeforeMount(() => {
  store.getEvents()
})

</script>

<template>
  <h1>Events</h1>

  <div v-if="store.events.length === 0">
    <Card>
      <template #title>
        No Upcoming Events at this Time
      </template>
      <template #content>
        Stay tuned for upcoming events!
      </template>
    </Card>
  </div>
  <EventList :is-read-only="true" />
</template>
