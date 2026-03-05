<script setup lang="ts">

import { type PropType, ref } from 'vue'
import type { EventScheduleItem } from '@/components/admin/eventScheduleItems/types'
import Button from 'primevue/button'
import {
  EventScheduleItemConfirmationPopup,
} from '@/components/admin/eventScheduleItems/services/eventScheduledItemConfirmationPopupService'
import EditEventScheduleItem from '@/components/admin/eventScheduleItems/EditEventScheduledItem.vue'
import type { DateTime } from 'luxon'
import type { Event } from '@/components/admin/events/types.ts'
import { userPermissionStore } from '@/stores/userPermissionStore.ts'

const props = defineProps({
  eventId: {
    type: Number,
    required: true,
  },
  event: {
    type: Object as PropType<Event>,
    required: true,
  },
  eventScheduleItem: {
    type: Object as PropType<EventScheduleItem>,
    required: true,
  },
  isReadOnly: {
    type: Boolean,
    required: true,
  },
})

const userPermissionInfo = userPermissionStore()
const permissionCheck = userPermissionInfo.permissionCheck

let popups = EventScheduleItemConfirmationPopup(props.eventScheduleItem.id, props.eventScheduleItem?.description)

const showEdit = ref(false)

function toggleEdit() {
  showEdit.value = !showEdit.value
}

function formatDate(date: DateTime) {
  return date.toFormat('t')
}

</script>

<template>
  <div v-if="showEdit && permissionCheck.EventScheduleItem.Edit" class="mb-2">
    <EditEventScheduleItem :event="props.event" :event-id="props.eventId" :event-schedule-item-id="props.eventScheduleItem.id" @canceled="toggleEdit" />
  </div>
  <div v-else class="d-flex flex-column flex-md-row align-self-center justify-content-between">
    <div class="flex-fill">
      {{ formatDate(props.eventScheduleItem?.startTime) }} - {{ formatDate(props.eventScheduleItem?.endTime) }} - {{ props.eventScheduleItem?.description }}
    </div>
    <div
      v-if="!showEdit && !props.isReadOnly"
      class="p-0 m-0 d-inline-flex align-items-start"
    >
      <Button
        v-if="permissionCheck.EventScheduleItem.Delete" class="mr-2" severity="danger" label="Delete" size="small"
        @click="popups.deleteConfirmation(props.eventId, $event)"
      />
      <Button v-if="permissionCheck.EventScheduleItem.Edit" class="float-end" label="Edit" size="small" @click="toggleEdit" />
    </div>
  </div>
</template>
