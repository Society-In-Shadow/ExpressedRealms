<script setup lang="ts">

import {type PropType, ref} from 'vue'
import Button from 'primevue/button'
import AddProgressionLevel from '@/components/expressions/progressionLevels/AddProgressionLevel.vue'
import type {ProgressionLevel} from '@/components/expressions/progressionPaths/types.ts'
import ShowProgressionLevel from '@/components/expressions/progressionLevels/ShowProgressionLevel.vue'

const props = defineProps({
  expressionId: {
    type: Number,
    required: true,
  },
  progressionId: {
    type: Number,
    required: true,
  },
  progressionLevels: {
    type: Array as PropType<ProgressionLevel[]>,
    required: true,
  },
})

const showAddPower = ref(false)

const toggleAddPower = () => {
  showAddPower.value = !showAddPower.value
}

</script>

<template>
  <div v-for="path in props.progressionLevels" :key="path.id">
    <ShowProgressionLevel :path="path" :expression-id="props.expressionId" :progression-id="props.progressionId" :is-read-only="false" />
  </div>

  <Button
    v-if="!showAddPower" class="w-100 m-2"
    label="Add Progression Levels" @click="toggleAddPower"
  />
  <AddProgressionLevel
    v-if="showAddPower"
    :expression-id="props.expressionId" :progression-id="props.progressionId" @canceled="toggleAddPower"
  />
</template>
