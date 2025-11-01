<script setup lang="ts">

import { onMounted, type PropType, ref } from 'vue'
import type { Event } from '@/components/admin/events/types'
import { UserRoles, userStore } from '@/stores/userStore'
import Button from 'primevue/button'
import Card from 'primevue/card'
import Tag from 'primevue/tag'
import { EventConfirmationPopup } from '@/components/admin/events/services/eventConfirmationPopupService'
import EditEvent from '@/components/admin/events/EditEvent.vue'
import { adminEventScheduleDialogs } from '@/components/admin/eventScheduleItems/services/dialogs.ts'
import type { DateTime } from 'luxon'

let userInfo = userStore()
const dialogs = adminEventScheduleDialogs()

const props = defineProps({
  event: {
    type: Object as PropType<Event>,
    required: true,
  },
  isReadOnly: {
    type: Boolean,
    required: true,
  },
})

let popups = EventConfirmationPopup(props.event.id, props.event.name)

const showEdit = ref(false)

const hasManageEventRole = ref(false)

onMounted(async () => {
  hasManageEventRole.value = await userInfo.hasUserRole(UserRoles.ManageEventRole)
})

function toggleEdit() {
  showEdit.value = !showEdit.value
}

function openMapWithFallback(address: string) {
  const encodedAddress = encodeURIComponent(address)

  window.open(`https://www.google.com/maps/search/?api=1&query=${encodedAddress}`, '_blank').focus()// = `https://www.google.com/maps/search/?api=1&query=${encodedAddress}`;
}

function formatDate(date: DateTime) {
  return date.toFormat('cccc, LLLL dd, yyyy')
}

</script>

<template>
  <Card>
    <template #content>
      <div v-if="showEdit" class="mb-2">
        <EditEvent :event-id="props.event.id" @canceled="toggleEdit" />
      </div>
      <div v-else class="d-flex flex-column flex-md-row align-self-center justify-content-between">
        <div>
          <h1 class="p-0 m-0">
            {{ props.event?.name }} <Tag v-if="hasManageEventRole" severity="secondary">
              {{ props.event?.isPublished ? "Published" : "Draft" }}
            </Tag>
          </h1>
          <div class="p-0 m-0">
            <div>{{ formatDate(props.event.startDate) }} - {{ formatDate(props.event.endDate) }}</div>
            <div><a href="" @click.prevent="openMapWithFallback(props.event.location)">{{ props.event.location }}</a></div>
            <div><a :href="props.event?.websiteUrl">{{ props.event.websiteName }}</a></div>
          </div>
        </div>
        <div
          class="p-0 m-0 d-inline-flex align-items-start"
        >
          <div class="mr-2">
            <Button class="" label="Schedule" @click="dialogs.showScheduleDialog(props.event.id, props.event, props.isReadOnly)" />
          </div>
          <div v-if="!showEdit && hasManageEventRole && !props.isReadOnly">
            <Button class="mr-2" severity="danger" label="Delete" @click="popups.deleteConfirmation($event)" />
            <Button v-if="props.event.isPublished == false" class="mr-2" severity="info" label="Publish" @click="popups.publishConfirmation($event)" />
            <Button class="float-end" label="Edit" @click="toggleEdit" />
          </div>
        </div>
      </div>
    </template>
  </Card>
</template>
