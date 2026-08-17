<script setup lang="ts">

import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import { computed, onBeforeMount } from 'vue'
import { formatDate } from '@/utilities/dateUtilities.ts'
import { EventQuestionStore } from '@/components/admin/eventQuestions/stores/eventQuestionStore.ts'
import type { QuestionResponse } from '@/components/admin/eventQuestions/types.ts'

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

const store = EventQuestionStore()

onBeforeMount(async () => {
  await store.getQuestionResponses(props.eventId)
})

const sortedItems = computed<QuestionResponse[]>(() => {
  return [...store.questionResponses].sort((a, b) => b.approvalDate - a.approvalDate)
})

</script>

<template>
  <DataTable :value="sortedItems">
    <Column field="approvalDate" header="Reviewed Date">
      <template #body="{data}">
        {{ formatDate(data.approvalDate) }}
      </template>
    </Column>
    <Column field="approver" header="Approver" />
    <Column field="playerName" header="Player Name" />
    <Column field="question" header="Question" />
    <Column field="answer" header="Answer" />
  </DataTable>
</template>
