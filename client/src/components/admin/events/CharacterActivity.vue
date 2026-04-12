<script setup lang="ts">

import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import { computed, onBeforeMount } from 'vue'
import type { AssignedXpInfo } from '@/components/admin/assignedXp/types.ts'
import { characterActivityStore } from '@/components/admin/events/stores/characterActivityStore.ts'
import { formatDate } from '@/utilities/dateUtilities.ts'

const props = defineProps({
  eventId: {
    type: Number,
    required: true,
  },
  isReadOnly: {
    type: Boolean,
    required: false,
    default: false,
  },
})

const store = characterActivityStore()

onBeforeMount(async () => {
  await store.getCharacterEvents(props.eventId)
})

const sortedItems = computed<AssignedXpInfo[]>(() => {
  return [...store.characterEvents].sort((a, b) => b.dateAssigned - a.dateAssigned)
})

</script>

<template>
  <DataTable :value="sortedItems">
    <Column field="dateAssigned" header="Date Assigned">
      <template #body="{data}">
        {{ formatDate(data.dateAssigned) }}
      </template>
    </Column>
    <Column header="Assigner">
      <template #body="{ data }">
        {{ data.assigner?.name }}
      </template>
    </Column>
    <Column header="Player / Character">
      <template #body="{data}">
        <span v-if="data.player">
          {{ data.player.name }}
        </span>
        <span v-if="data.character">
          - {{ data.character.name }}
        </span>
      </template>
    </Column>
    <Column header="XP Type">
      <template #body="{ data }">
        {{ data.xpType?.name }}
      </template>
    </Column>
    <Column field="amount" header="Amount" />
    <Column field="notes" header="Notes" />
  </DataTable>
</template>
