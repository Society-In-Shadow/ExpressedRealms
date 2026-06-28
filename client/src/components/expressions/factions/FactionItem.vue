<script setup lang="ts">

import { onMounted, type PropType } from 'vue'
import Card from 'primevue/card'
import { ConfirmationPopup } from './services/confirmationPopupService'
import { useRouter } from 'vue-router'
import SplitButton from 'primevue/splitbutton'
import { userPermissionStore } from '@/stores/userPermissionStore.ts'
import type { Faction } from '@/components/expressions/factions/types.ts'
import { factionDialogs } from '@/components/expressions/factions/services/dialogs.ts'

const userPermissionInfo = userPermissionStore()
const permissionCheck = userPermissionInfo.permissionCheck
const router = useRouter()

const props = defineProps({
  item: {
    type: Object as PropType<Faction>,
    required: true,
  },
})

let popups = ConfirmationPopup(props.item.id, props.item.name)
let editItemPopup = factionDialogs()

const items = []

onMounted(async () => {
  if (permissionCheck.Faction.Delete) {
    items.push({
      label: 'Delete',
      command: ($event) => {
        popups.deleteConfirmation($event)
      },
    })
  }
  if (permissionCheck.Faction.Edit) {
    items.push({
      label: 'Edit',
      command: ($event) => {
        editItemPopup.showUpdateFaction(props.item.id)
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
            <div v-html="props.item.background" />
          </div>
        </div>
        <div class="p-0 m-0 d-inline-flex align-items-start">
          <SplitButton label="View" severity="info" :model="items" @click="toggleEdit()" />
        </div>
      </div>
    </template>
  </Card>
</template>
