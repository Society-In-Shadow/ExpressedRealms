<script setup lang="ts">

import { onBeforeMount } from 'vue'
import Button from 'primevue/button'
import DataTable from 'primevue/datatable'
import { formatDate } from '@/utilities/dateUtilities.ts'
import Column from 'primevue/column'
import { roleUserAssignmentDialogs } from '@/components/admin/roles/assignedUsers/services/dialogs.ts'
import { ConfirmationPopup } from '@/components/admin/roles/assignedUsers/services/confirmationPopupService.ts'
import { assignedUsersStore } from '@/components/admin/roles/assignedUsers/stores/assignedUsersStore.ts'

const assignedUserData = assignedUsersStore()
const popup = roleUserAssignmentDialogs()
const confirmation = ConfirmationPopup()

const props = defineProps({
  roleId: {
    type: Number,
    required: true,
  },
})

onBeforeMount(async () => {
  await assignedUserData.getAssignedUsers(props.roleId)
})

</script>

<template>
  <div class="text-right">
    <Button label="Add User" class="m-2" @click="popup.showAddUserPopup()" />
  </div>
  <DataTable :value="assignedUserData.userRoles">
    <Column field="name" header="User / Email" />
    <Column field="expireDate" header="Expire Date">
      <template #body="{data}">
        <div v-if="data.expireDate">
          {{ formatDate(data.expireDate) }}
        </div>
        <div v-else>
          <span class="material-symbols-outlined">all_inclusive</span>
        </div>
      </template>
    </Column>
    <Column>
      <template #body="{data}">
        <Button label="Remove" severity="danger" class="m-2 float-end" @click="confirmation.removeUserConfirmation($event, props.roleId, data.userId, data.name)" />
      </template>
    </Column>
  </DataTable>
</template>
