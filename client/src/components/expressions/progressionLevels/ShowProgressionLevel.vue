<script setup lang="ts">

import { type PropType, ref } from 'vue'
import Button from 'primevue/button'
import EditProgressionLevel from '@/components/expressions/progressionLevels/EditProgressionLevel.vue'
import {
  progressionLevelConfirmationPopupService,
} from '@/components/expressions/progressionLevels/services/progressionLevelConfirmationPopupService.ts'
import type { ProgressionLevel } from '@/components/expressions/progressionPaths/types.ts'
import { can } from '@/stores/userPermissionStore.ts'

const showEdit = ref(false)
const toggleEdit = () => {
  showEdit.value = !showEdit.value
}

const props = defineProps({
  expressionId: {
    type: Number,
    required: true,
  },
  progressionId: {
    type: Number,
    required: true,
  },
  path: {
    type: Object as PropType<ProgressionLevel>,
    required: true,
  },
  isReadOnly: {
    type: Boolean,
    required: false,
  },
})

const popups = progressionLevelConfirmationPopupService(props.progressionId, props.path.name)

</script>

<template>
  <div v-if="!showEdit">
    <div class="d-flex flex-column flex-md-row align-self-center justify-content-between">
      <h1 class="p-0 m-0">
        XL {{ props.path.xlLevel }}
      </h1>
      <div v-if="(can.ProgressionPath.Delete || can.ProgressionPath.Edit) && !props.isReadOnly" class="d-inline-flex align-items-start">
        <Button v-if="can.ProgressionPath.Delete" class="m-2" severity="danger" label="Delete" @click="popups.deleteConfirmation($event, props.path.id)" />
        <Button v-if="can.ProgressionPath.Edit" label="Edit" class="float-end m-2" @click="toggleEdit()" />
      </div>
    </div>
    <div class="mb-0 pb-0" v-html="props.path.description" />
  </div>
  <div v-else>
    <EditProgressionLevel :progression-id="props.progressionId" :expression-id="props.expressionId" :level="props.path" @cancelled="toggleEdit()" />
  </div>
</template>
