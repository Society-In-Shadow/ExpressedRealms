<script setup lang="ts">

import { EventStore } from '@/components/admin/events/stores/eventStore'
import { computed, onBeforeMount, ref } from 'vue'
import EventItem from '@/components/admin/events/EventItem.vue'
import { UserRoles, userStore } from '@/stores/userStore'
import Button from 'primevue/button'
import AddEvent from '@/components/admin/events/AddEvent.vue'
import { DateTime } from 'luxon'

import Tabs from 'primevue/tabs'
import TabList from 'primevue/tablist'
import Tab from 'primevue/tab'
import TabPanels from 'primevue/tabpanels'
import TabPanel from 'primevue/tabpanel'
import type { Event } from '@/components/admin/events/types.ts'

const store = EventStore()
const userInfo = userStore()
const hasEventManagementRole = ref(false)

onBeforeMount(async () => {
  await store.getEvents()
  hasEventManagementRole.value = await userInfo.hasUserRole(UserRoles.ManageEventRole)
})

const showAdd = ref(false)

const toggleAdd = () => {
  showAdd.value = !showAdd.value
}

const props = defineProps({
  isReadOnly: {
    type: Boolean,
    required: true,
  },
})

// Create a computed property for sorted Events
const sortedEvents = computed<Event[]>(() => {
  return [...store.events].sort((a, b) => {
    // Assuming you want to sort by a property like 'title' or 'name'
    // Replace 'title' with the actual property you want to sort by
    const nameA = a.name?.toLowerCase() || ''
    const nameB = b.name?.toLowerCase() || ''
    return nameA.localeCompare(nameB)
  })
})
const currentDate = DateTime.now()
const previousEvents = computed(() => {
  return sortedEvents.value.filter(x => x.endDate < currentDate && x.isPublished)
})

const currentAndUnpublishedEvents = computed(() => {
  return sortedEvents.value.filter(x => !(x.endDate < currentDate && x.isPublished))
})

</script>

<template>
  <Tabs value="0">
    <TabList>
      <Tab value="0">
        Upcoming / Current Events
      </Tab>
      <Tab value="1">
        Previous Events
      </Tab>
    </TabList>
    <TabPanels>
      <TabPanel value="0">
        <div v-for="event in currentAndUnpublishedEvents" :key="event.id" class="py-3">
          <EventItem :event="event" :is-read-only="props.isReadOnly" />
        </div>
      </TabPanel>
      <TabPanel value="1">
        <div v-for="event in previousEvents" :key="event.id" class="py-3">
          <EventItem :event="event" :is-read-only="props.isReadOnly" />
        </div>
      </TabPanel>
    </TabPanels>
  </Tabs>

  <AddEvent v-if="showAdd && hasEventManagementRole && !props.isReadOnly" @canceled="toggleAdd" />
  <Button
    v-if="!showAdd && hasEventManagementRole && !props.isReadOnly" class="w-100 m-2"
    label="Add Event" @click="toggleAdd"
  />
</template>
