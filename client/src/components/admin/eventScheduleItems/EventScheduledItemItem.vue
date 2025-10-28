<script setup lang="ts">

import { onMounted, type PropType, ref } from 'vue'
import type { EventScheduleItem } from '@/components/admin/eventScheduleItems/types'
import { UserRoles, userStore } from '@/stores/userStore'
import Button from 'primevue/button'
import {
  EventScheduleItemConfirmationPopup,
} from '@/components/admin/eventScheduleItems/services/eventScheduledItemConfirmationPopupService'
import EditEventScheduleItem from '@/components/admin/eventScheduleItems/EditEventScheduledItem.vue'

let userInfo = userStore()

const props = defineProps({
  eventId: {
    type: Number,
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

let popups = EventScheduleItemConfirmationPopup(props.eventScheduleItem.id, props.eventScheduleItem?.description)

const showEdit = ref(false)

const hasManageEventScheduleItemRole = ref(false)

onMounted(async () => {
  hasManageEventScheduleItemRole.value = userInfo.hasUserRole(UserRoles.ManageEventRole)
})

function toggleEdit() {
  showEdit.value = !showEdit.value
}

function formatDate(date: string) {
  return new Date(date).toLocaleTimeString('en-US', {
    hour12: true,
    hour: 'numeric',
    minute: 'numeric',
  })
}

</script>

<template>
  <div v-if="showEdit" class="mb-2">
    <EditEventScheduleItem :event-id="props.eventId" :event-schedule-item-id="props.eventScheduleItem.id" @canceled="toggleEdit" />
  </div>
  <div v-else class="d-flex flex-column flex-md-row align-self-center justify-content-between">
    <div class="flex-fill">
      {{ formatDate(props.eventScheduleItem?.startTime) }} - {{ formatDate(props.eventScheduleItem?.endTime) }} - {{ props.eventScheduleItem?.description }}
    </div>
    <div
      v-if="!showEdit && hasManageEventScheduleItemRole && !props.isReadOnly"
      class="p-0 m-0 d-inline-flex align-items-start"
    >
      <Button class="mr-2" severity="danger" label="Delete" size="small" @click="popups.deleteConfirmation(props.eventId, $event)" />
      <Button class="float-end" label="Edit" size="small" @click="toggleEdit" />
    </div>
  </div>
</template>
