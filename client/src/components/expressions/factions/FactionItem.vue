<script setup lang="ts">

import { onMounted, type PropType, ref } from 'vue'
import Card from 'primevue/card'
import { ConfirmationPopup } from './services/confirmationPopupService'
import { userPermissionStore } from '@/stores/userPermissionStore.ts'
import type { Faction } from '@/components/expressions/factions/types.ts'
import { factionDialogs } from '@/components/expressions/factions/services/dialogs.ts'
import CommandButton, { type Command } from '@/uiComponents/CommandButton.vue'

const userPermissionInfo = userPermissionStore()
const permissionCheck = userPermissionInfo.permissionCheck

const props = defineProps({
  item: {
    type: Object as PropType<Faction>,
    required: true,
  },
})

let popups = ConfirmationPopup(props.item.id, props.item.name)
let editItemPopup = factionDialogs()

const items = ref<Command[]>([])

onMounted(async () => {
  if (permissionCheck.Faction.Edit) {
    items.value.push({
      label: 'Edit',
      severity: 'primary',
      command: ($event) => {
        editItemPopup.showUpdateFaction(props.item.id)
      },
    })
  }
  if (permissionCheck.Faction.Delete) {
    items.value.push({
      label: 'Delete',
      command: ($event) => {
        popups.deleteConfirmation($event)
      },
      severity: 'danger',
    })
  }
})

</script>

<template>
  <Card>
    <template #content>
      <div class="d-flex flex-column flex-md-row align-self-center justify-content-between">
        <h1 class="p-0 m-0 flex-fill">
          {{ props.item?.name }}
        </h1>
        <div class="p-0 m-0 d-inline-flex align-items-start">
          <CommandButton :commands="items" />
        </div>
      </div>
      <div class="p-0 m-0">
        <div v-html="props.item.background" />
      </div>

      <div v-for="level in props.item.factionLevels" :key="level.id">
        <h2>{{ level.rankName }} Rank</h2>
        <h3>Requirements:</h3>
        <div v-if="level.rankName == 'Basic'">
          No Requirements to join
        </div>
        <div v-else>
          <p>Knowledge in {{ level.knowledge }} at a {{ level.knowledgeLevel }} level with a specialization in {{ level.specialization }}.</p>
        </div>
        <h2>Rank Power</h2>
        <div>
          Fancy Power Info
        </div>
      </div>
    </template>
  </Card>
</template>
