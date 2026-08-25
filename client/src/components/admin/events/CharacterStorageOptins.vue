<script setup lang="ts">

import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import { computed } from 'vue'
import { formatDate } from '@/utilities/dateUtilities.ts'
import { useQuery } from '@pinia/colada'
import { characterStorageEventList } from '@/components/admin/events/stores/characterStorageStore.ts'
import type { CharacterStorageOptin } from '@/components/admin/events/types.ts'

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

const { data, isLoading } = useQuery(characterStorageEventList(props.eventId))

const sortedItems = computed<CharacterStorageOptin[]>(() => {
  if (!data.value)
    return []

  return [...data.value.characterStorageOptins].sort((a, b) => b.timestampAssigned - a.timestampAssigned)
})

</script>

<template>
  <DataTable :value="sortedItems" :loading="isLoading">
    <Column field="timestamp" header="Date Assigned">
      <template #body="{data}">
        {{ formatDate(data.timestampAssigned) }}
      </template>
    </Column>
    <Column field="approverName" header="Collector" />
    <Column field="playerName" header="Player" />
    <Column field="amount" header="Amount" />
  </datatable>
</template>
