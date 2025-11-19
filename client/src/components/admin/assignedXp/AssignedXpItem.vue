<script setup lang="ts">

import { onMounted, type PropType, ref } from 'vue'
import { UserRoles, userStore } from '@/stores/userStore'
import Button from 'primevue/button'
import Card from 'primevue/card'
import type { AssignedXpInfo } from '@/components/admin/assignedXp/types.ts'
import EditAssignedXp from '@/components/admin/assignedXp/EditAssignedXp.vue'
import {
  xpAssignmentConfirmationPopup,
} from '@/components/admin/assignedXp/services/xpAssignmentConfirmationPopupService.ts'

let userInfo = userStore()

const props = defineProps({
  item: {
    type: Object as PropType<AssignedXpInfo>,
    required: true,
  },
  characterId: {
    type: Number,
    required: true,
  },
  isReadOnly: {
    type: Boolean,
    required: true,
  },
})

let popups = xpAssignmentConfirmationPopup()

const showEdit = ref(false)

const hasManageEventRole = ref(false)
const isEvent = ref(false)

onMounted(async () => {
  hasManageEventRole.value = await userInfo.hasUserRole(UserRoles.ManageEventRole)
  isEvent.value = props.item.xpType.id == 1
})

function toggleEdit() {
  showEdit.value = !showEdit.value
}

</script>

<template>
  <Card>
    <template #content>
      <div v-if="showEdit && !isEvent" class="mb-2">
        <EditAssignedXp :id="props.item.id" :character-id="props.characterId" @canceled="toggleEdit" />
      </div>
      <div v-else class="d-flex flex-column align-self-center justify-content-end">
        <div class="flex-fill mr-3">
          <div>
            {{ props.item.event.name }} - {{ props.item.assigner?.name }}
          </div>
          <div>
            {{ props.item.xpType.name }} - {{ props.item.amount }}
          </div>
          <div>
            {{ props.item.notes }}
          </div>
        </div>
      </div>
      <div v-if="!showEdit && hasManageEventRole && !props.isReadOnly && !isEvent" class="p-0 m-0 float-end">
        <Button class="mr-2" severity="danger" label="Delete" @click="popups.deleteConfirmation($event, props.item.id, props.characterId)" />
        <Button label="Edit" @click="toggleEdit" />
      </div>
    </template>
  </Card>
</template>
