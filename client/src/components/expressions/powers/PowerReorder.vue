<script setup lang="ts">

import { powersStore } from '@/components/expressions/powers/stores/powersStore'
import { Drag, DropList } from 'vue-easy-dnd'
import Button from 'primevue/button'
import { ref } from 'vue'
import axios from 'axios'
import { getSortAndIdsForPowerPaths } from '@/components/expressions/powerPaths/utilities/powerPathUtilities'
import toaster from '@/services/Toasters'
import type { Power } from '@/components/expressions/powers/types'

let powerInfo = powersStore()

const props = defineProps({
  powerPathId: {
    type: Number,
    required: true,
  },
  powers: {
    type: Array as () => Power[],
    required: true,
  },
  isReadOnly: {
    type: Boolean,
    required: false,
  },
})

const emit = defineEmits<{
  togglePreview: []
}>()

function saveChanges() {
  axios.put(`/powerpath/${props.powerPathId}/updateSorting`, {
    expressionId: props.powerPathId,
    items: getSortAndIdsForPowerPaths(props.powers),
  }).then(() => {
    emit('togglePreview')
    showPowerPathReorder.value = !showPowerPathReorder.value
    toaster.success('Successfully Updated Power Sorting!')
  })
}

const showPowerPathReorder = ref(false)
function toggleEdit() {
  if (showPowerPathReorder.value)
    powerInfo.updatePowersByPathId(props.powerPathId)

  emit('togglePreview')
  showPowerPathReorder.value = !showPowerPathReorder.value
}

</script>

<template>
  <div class="row">
    <Button class="col m-2" :label="showPowerPathReorder ? 'Cancel' : 'Reorder Powers'" @click="toggleEdit" />
    <Button v-if="showPowerPathReorder" label="Save" class="col m-2" @click="saveChanges" />
  </div>

  <drop-list v-if="showPowerPathReorder" :items="props.powers" @reorder="$event.apply(props.powers)">
    <template #item="{item}">
      <drag :key="item.id" :data="item">
        <h1><i class="pi pi-bars mr-2" />{{ item.name }}</h1>
      </drag>
    </template>
    <template #feedback="{data}">
      <h1><i class="pi pi-bars mr-2" />{{ data.name }}</h1>
    </template>
  </drop-list>
</template>
