<script setup lang="ts">
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
  <EventList :is-read-only="true" />
</template>
