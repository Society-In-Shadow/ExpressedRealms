<script setup lang="ts">

import { type PropType, ref } from 'vue'
import type { Blessing } from '@/components/blessings/types'
import EditBlessing from '@/components/blessings/EditBlessing.vue'
import Button from 'primevue/button'
import { blessingConfirmationPopup } from '@/components/blessings/services/blessingConfirmationPopupService.ts'
import { addBlessingDialog } from '@/components/blessings/services/dialogs.ts'
import { makeIdSafe } from '@/utilities/stringUtilities.ts'
import { can } from '@/stores/userPermissionStore.ts'

const popups = blessingConfirmationPopup()
const dialogs = addBlessingDialog()
const props = defineProps({
  blessing: {
    type: Object as PropType<Blessing>,
    required: true,
  },
  isReadOnly: {
    type: Boolean,
    required: true,
  },
})

const showEdit = ref(false)

function toggleEdit() {
  showEdit.value = !showEdit.value
}
</script>

<template>
  <div v-if="showEdit" class="mb-2">
    <EditBlessing :blessing="props.blessing" @canceled="toggleEdit" />
  </div>
  <div class="d-flex flex-column flex-md-row align-self-center justify-content-between mt-4">
    <div>
      <h3 :id="makeIdSafe(props.blessing.name)" class="p-0 m-0">
        {{ props.blessing.name }}
      </h3>
    </div>
    <div
      v-if="!showEdit && (can.Blessings.Edit || can.Blessings.Delete) && !props.isReadOnly"
      class="p-0 m-0 d-inline-flex align-items-start"
    >
      <Button v-if="can.Blessings.Delete" class="mr-2" severity="danger" label="Delete" @click="popups.deleteConfirmation($event, props.blessing.id)" />
      <Button v-if="can.Blessings.Edit" class="float-end" label="Edit" @click="toggleEdit" />
    </div>
  </div>
  <div v-html="props.blessing?.description" />
  <ul>
    <li v-for="level in props.blessing.levels" :key="level.id" class="mt-3">
      <div class="d-flex flex-column flex-md-row align-self-center justify-content-between">
        <div>{{ level.name }} – {{ level.description }}</div>
        <div
          v-if="!showEdit && (can.Blessings.Edit || can.Blessings.Delete) && !props.isReadOnly"
          class="p-0 m-0 d-inline-flex align-items-start"
        >
          <Button v-if="can.Blessings.Delete" class="mr-2" severity="danger" label="Delete" @click="popups.deleteBlessingLevelConfirmation($event, props.blessing.id, level.id)" />
          <Button v-if="can.Blessings.Edit" class="float-end" label="Edit" @click="dialogs.showEditBlessingLevel(props.blessing.id, level.id, level.modifierGroupId)" />
        </div>
      </div>
    </li>
    <li v-if="can.Blessings.Create">
      <Button label="Add Level" @click="dialogs.showAddBlessingLevel(props.blessing.id)" />
    </li>
  </ul>
</template>
