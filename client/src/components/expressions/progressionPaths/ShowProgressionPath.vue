<script setup lang="ts">

import { ref } from 'vue'
import Button from 'primevue/button'
import { makeIdSafe } from '@/utilities/stringUtilities'
import EditProgressionPath from '@/components/expressions/progressionPaths/EditProgressionPath.vue'
import {
  progressionPathConfirmationPopupService,
} from '@/components/expressions/progressionPaths/services/powerPathConfirmationPopupService.ts'
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
  path: {
    type: Object,
    required: true,
  },
  isReadOnly: {
    type: Boolean,
    required: false,
  },
})

const popups = progressionPathConfirmationPopupService(props.path.id, props.path.name)

</script>

<template>
  <div v-if="!showEdit" :id="makeIdSafe(props.path.name)">
    <div class="d-flex flex-column flex-md-row align-self-center justify-content-between">
      <h1 class="p-0 m-0">
        {{ props.path.name }}
      </h1>
      <div v-if="(can.ProgressionPath.Edit || can.ProgressionPath.Delete) && !props.isReadOnly" class="d-inline-flex align-items-start">
        <Button v-if="can.ProgressionPath.Delete" class="m-2" severity="danger" label="Delete" @click="popups.deleteConfirmation($event)" />
        <Button v-if="can.ProgressionPath.Edit" label="Edit" class="float-end m-2" @click="toggleEdit()" />
      </div>
    </div>
    <div class="mb-0 pb-0" v-html="props.path.description" />
  </div>
  <div v-else>
    <EditProgressionPath :progression-id="props.path.id" :expression-id="props.expressionId" @cancelled="toggleEdit()" />
  </div>
</template>
