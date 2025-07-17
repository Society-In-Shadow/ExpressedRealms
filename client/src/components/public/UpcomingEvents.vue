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
    <template #title>No Upcoming Events at this Time</template>
    <template #content>Stay tuned for upcoming events!</template>
  </Card>
</div>
<div v-for="event in store.events">
  <Card>
    <template #title><h1 class="m-0">{{event.name}}</h1></template>
    <template #content>
      <div>{{event.startDate.toDateString()}} - {{event.endDate.toDateString()}}</div>
      <div>{{event.location}}</div>
      <div><a :href="event.conWebsiteUrl">{{event.conWebsiteName}}</a></div>
      <h1 class="m-0 pt-5" v-if="event.staff">Meet Our Team</h1>
      <div v-for="staff in event.staff" class="pt-2">
        <div class="d-flex flex-column flex-md-row align-items-center">
          <div>
            <h2>{{staff.name}}</h2>
            <p>{{staff.bio}}</p>
          </div>
        </div>
      </div>
    </template>
  </Card>
</div>

</template>

<style scoped>

</style>