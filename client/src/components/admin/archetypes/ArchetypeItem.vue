<script setup lang="ts">

import { onMounted, type PropType } from 'vue'
import Card from 'primevue/card'
import { ConfirmationPopup } from './services/confirmationPopupService'
import { useRouter } from 'vue-router'
import SplitButton from 'primevue/splitbutton'
import type { Archetype } from './types.ts'
import { userPermissionStore } from '@/stores/userPermissionStore.ts'

const userPermissionInfo = userPermissionStore()
const permissionCheck = userPermissionInfo.permissionCheck
const router = useRouter()

const props = defineProps({
  item: {
    type: Object as PropType<Archetype>,
    required: true,
  },
})

let popups = ConfirmationPopup(props.item.id, props.item.name)

const items = []

onMounted(async () => {
  if (permissionCheck.Archetypes.Delete) {
    items.push({
      label: 'Delete',
      command: ($event) => {
        popups.deleteConfirmation($event)
      },
    })
  }
})

async function toggleEdit() {
  await router.push({ name: 'characterSheet', params: { id: props.item.id } })
}
</script>

<template>
  <Card>
    <template #content>
      <div class="d-flex flex-column flex-md-row align-self-center justify-content-between">
        <div>
          <h1 class="p-0 m-0">
            {{ props.item?.name }}
          </h1>
          <div class="p-0 m-0">
            <div>{{ props.item.description }}</div>
          </div>
        </div>
        <div class="p-0 m-0 d-inline-flex align-items-start">
          <SplitButton label="View" severity="info" :model="items" @click="toggleEdit()" />
        </div>
      </div>
    </template>
  </Card>
</template>
