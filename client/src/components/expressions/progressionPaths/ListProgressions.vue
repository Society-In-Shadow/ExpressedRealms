<script setup lang="ts">

import { onBeforeMount, ref } from 'vue'
import Button from 'primevue/button'
import Divider from 'primevue/divider'
import { progressionPathStore } from '@/components/expressions/progressionPaths/stores/progressionPathsStore.ts'
import AddProgressionPath from '@/components/expressions/progressionPaths/AddProgressionPath.vue'
import ShowProgressionPath from '@/components/expressions/progressionPaths/ShowProgressionPath.vue'
import ListProgressionLevels from '@/components/expressions/progressionLevels/ListProgressionLevels.vue'
import { can } from '@/stores/userPermissionStore.ts'

let progressionPaths = progressionPathStore()

const props = defineProps({
  expressionId: {
    type: Number,
    required: true,
  },
})

onBeforeMount(async () => {
  await progressionPaths.getProgressionPaths(props.expressionId)
})

const showAddPower = ref(false)

const toggleAddPower = () => {
  showAddPower.value = !showAddPower.value
}

</script>

<template>
  <div v-for="path in progressionPaths.progressionPaths" :key="path.id">
    <ShowProgressionPath :path="path" :expression-id="props.expressionId" :is-read-only="false" />

    <Divider />
    <h2>Levels</h2>
    <ListProgressionLevels :expression-id="props.expressionId" :progression-id="path.id" :progression-levels="path.levels" />
  </div>

  <Button
    v-if="!showAddPower && can.ProgressionPath.Create" class="w-100 m-2"
    label="Add Progression Path" @click="toggleAddPower"
  />
  <AddProgressionPath
    v-if="showAddPower && can.ProgressionPath.Create"
    :expression-id="props.expressionId" @canceled="toggleAddPower"
  />
</template>
