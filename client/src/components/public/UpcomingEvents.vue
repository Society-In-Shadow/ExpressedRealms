<script setup lang="ts">
import Card from 'primevue/card';

import {onBeforeMount} from "vue";
import {eventStore} from "@/components/public/stores/eventStore";

const store = eventStore();

onBeforeMount(() => {
  store.getEvents();
})
</script>

<template>
  <h1>Our Upcoming Events!</h1>
  
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
  <div v-for="event in store.events" :key="event.id">
    <Card>
      <template #title>
        <h1 class="m-0">
          {{ event.name }}
        </h1>
      </template>
      <template #content>
        <div>{{ event.startDate.toDateString() }} - {{ event.endDate.toDateString() }}</div>
        <div><a :href="`maps:${event.location}`">{{ event.location }}</a></div>
        <div><a :href="event.conWebsiteUrl">{{ event.conWebsiteName }}</a></div>
      </template>
    </Card>
  </div>
</template>
