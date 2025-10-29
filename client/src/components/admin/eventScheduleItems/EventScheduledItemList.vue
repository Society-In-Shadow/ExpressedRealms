<script setup lang="ts">

import { EventScheduleItemStore } from '@/components/admin/eventScheduleItems/stores/eventScheduleItemStore'
import { computed, inject, onBeforeMount, type Ref, ref } from 'vue'
import EventScheduleItemItem from '@/components/admin/eventScheduleItems/EventScheduledItemItem.vue'
import { UserRoles, userStore } from '@/stores/userStore'
import Button from 'primevue/button'
import AddEventScheduledItem from '@/components/admin/eventScheduleItems/AddEventScheduledItem.vue'
import { DateTime } from 'luxon'
import { Message } from 'primevue'
import { EventStore } from '@/components/admin/events/stores/eventStore.ts'

const store = EventScheduleItemStore()
const eventInfo = EventStore()
const userInfo = userStore()
const hasEventScheduleItemManagementRole = ref(false)

const dialogRef = inject('dialogRef') as Ref
const eventId = ref(dialogRef.value.data.eventId)
const event = ref(dialogRef.value.data.event)

onBeforeMount(async () => {
  await store.getEventScheduleItems(eventId.value)
  hasEventScheduleItemManagementRole.value = userInfo.hasUserRole(UserRoles.ManageEventRole)
})

const showAdd = ref(false)

const toggleAdd = () => {
  showAdd.value = !showAdd.value
}

// Create a computed property for sorted EventScheduleItems
const sortedEventScheduleItems = computed(() => {
  if (store.eventScheduleItems[eventId.value] === undefined) return []

  return [...store.eventScheduleItems[eventId.value]].sort((a, b) => {
    // Assuming you want to sort by a property like 'title' or 'name'
    // Replace 'title' with the actual property you want to sort by
    const nameA = a.description?.toLowerCase() || ''
    const nameB = b.description?.toLowerCase() || ''
    return nameA.localeCompare(nameB)
  })
})

function groupAndSortScheduleItems(
  items: EventScheduleItem[],
): { date: string, events: EventScheduleItem[] }[] {
  // First, group by date (YYYY-MM-DD so we can sort easily)
  const grouped = items.reduce<Record<string, EventScheduleItem[]>>((acc, item) => {
    const dateKey = item.startTime.toFormat('yyyy-MM-dd')
    if (!acc[dateKey]) acc[dateKey] = []
    acc[dateKey].push(item)
    return acc
  }, {})

  // Then, turn into an array and sort by date
  return Object.entries(grouped)
    .map(([dateKey, dayItems]) => {
      // Sort each day's events by start time
      const sortedEvents = dayItems.sort(
        (a, b) =>
          a.startTime - b.startTime,
      )

      // Create a readable label
      const formattedDate = DateTime.fromISO(dateKey).toFormat('cccc, MMM dd')

      return { date: formattedDate, events: sortedEvents }
    })
  // Sort by chronological date
    .sort(
      (a, b) =>
        a.events[0].startTime - b.events[0].startTime,
    )
}

const timezone = computed(() => {
  return eventInfo.timeZones.find(x => x.id == event.value.timeZoneId)?.name
})

</script>

<template>
  <Message severity="info">
    {{ event.name }} is in the <strong>{{ timezone }}</strong> and our schedule below reflects that.
  </Message>
  <div v-for="day in groupAndSortScheduleItems(sortedEventScheduleItems)" :key="day.date" class="w-100">
    <h2>{{ day.date }}</h2>
    <div v-for="EventScheduleItem in day.events" :key="EventScheduleItem.id" class="py-3 w-100">
      <EventScheduleItemItem :event="event" :event-id="eventId" :event-schedule-item="EventScheduleItem" :is-read-only="false" />
    </div>
  </div>

  <AddEventScheduledItem v-if="showAdd && hasEventScheduleItemManagementRole" :event="event" :event-id="eventId" @canceled="toggleAdd" />
  <Button
    v-if="!showAdd && hasEventScheduleItemManagementRole" class="w-100 m-2"
    label="Add Schedule Item" @click="toggleAdd"
  />
</template>
