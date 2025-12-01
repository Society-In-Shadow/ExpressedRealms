<script setup lang="ts">

import { onMounted, type PropType, ref } from 'vue'
import type { Event } from '@/components/admin/events/types'
import { UserRoles, userStore } from '@/stores/userStore'
import Button from 'primevue/button'
import Card from 'primevue/card'
import Tag from 'primevue/tag'
import { EventConfirmationPopup } from '@/components/admin/events/services/eventConfirmationPopupService'
import { adminEventScheduleDialogs } from '@/components/admin/eventScheduleItems/services/dialogs.ts'
import { DateTime } from 'luxon'
import { useRouter } from 'vue-router'
import SplitButton from 'primevue/splitbutton'

let userInfo = userStore()
const router = useRouter()
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

const items = [
  {
    label: 'Delete',
    command: ($event) => {
      popups.deleteConfirmation($event)
    },
  },
]

onMounted(async () => {
  hasManageEventRole.value = await userInfo.hasUserRole(UserRoles.ManageEventRole)

  if (!props.event.isPublished) {
    items.push({
      label: 'Publish',
      command: ($event) => {
        popups.publishConfirmation($event)
      },
    })
  }
})

async function toggleEdit() {
  await router.push({ name: 'editEvent', params: { id: props.event.id } })
}

function openMapWithFallback(address: string) {
  const encodedAddress = encodeURIComponent(address)

  window.open(`https://www.google.com/maps/search/?api=1&query=${encodedAddress}`, '_blank').focus()
}

function formatDate(date: DateTime) {
  return date.toFormat('cccc, LLLL dd, yyyy')
}

</script>

<template>
  <Card>
    <template #content>
      <div class="d-flex flex-column flex-md-row align-self-center justify-content-between">
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
        <div class="p-0 m-0 d-inline-flex align-items-start">
          <div v-if="hasManageEventRole || props.event?.startDate <= DateTime.now().plus({ months: 1 })" class="mr-2">
            <Button label="Schedule" @click="dialogs.showScheduleDialog(props.event.id, props.event, true)" />
          </div>
          <div v-if="!showEdit && hasManageEventRole && !props.isReadOnly">
            <SplitButton label="View" severity="info" :model="items" @click="toggleEdit()" />
          </div>
        </div>
      </div>
    </template>
  </Card>
</template>
