<script setup lang="ts">

import { AssignedXpStore } from '@/components/admin/assignedXp/stores/assignedXpStore'
import { computed, onBeforeMount } from 'vue'
import type { AssignedXpInfo } from '@/components/admin/assignedXp/types.ts'

const props = defineProps({
  characterId: {
    type: Number,
    required: true,
  },
  isReadOnly: {
    type: Boolean,
    required: false,
    default: false,
  },
})

const store = AssignedXpStore()

onBeforeMount(async () => {
  await store.getAssignedXp(props.characterId)
})

const sortedItems = computed<AssignedXpInfo[]>(() => {
  return [...store.assignedXpItems].sort((a, b) => b.dateAssigned - a.dateAssigned)
})

const events = computed(() => {
  return Object.groupBy(sortedItems.value, item => item.event.id)
})

const eventSorted = computed<AssignedXpInfo[][]>(() => {
  return [...Object.values(events.value)].sort((a: AssignedXpInfo[], b: AssignedXpInfo[]) => {
    return b.find((x: AssignedXpInfo) => x.xpType.id == 1)!.dateAssigned - a.find((x: AssignedXpInfo) => x.xpType.id == 1)!.dateAssigned
  })
})

</script>

<template>
  <div class="w-100 mx-auto" style="max-width: 25em;">
    <div class="d-flex flex-row align-content-between">
      <div class="flex-fill" />
      <div>
        Amount
      </div>
    </div>
    <div v-for="(event) of eventSorted" :key="event[0].event.id">
      <div>{{ event[0].event.name }}</div>
      <div v-for="item of event" :key="item.id" class="ml-3">
        <div class="d-flex flex-row align-content-between w-100">
          <div class="flex-fill">
            {{ item.xpType.name }} <span v-if="item.xpType.id == 3">
              - {{ item.notes }}
            </span>
          </div>
          <div>{{ item.amount }}</div>
        </div>
      </div>
    </div>
    <div class="d-flex flex-row align-content-between">
      <div class="flex-fill">
        Total
      </div>
      <div style="border-top: white solid 2px">
        {{ store.assignedXpItems.reduce((sum, current) => sum + current.amount, 0) }}
      </div>
    </div>
  </div>
</template>
