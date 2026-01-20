<script setup lang="ts">

import { onBeforeMount } from 'vue'
import Button from 'primevue/button'
import DataTable from 'primevue/datatable'
import { formatDate } from '@/utilities/dateUtilities.ts'
import Column from 'primevue/column'
import { usrRoleAssignmentDialogs } from '@/components/admin/players/assignedRoles/services/dialogs.ts'
import { ConfirmationPopup } from '@/components/admin/players/assignedRoles/services/confirmationPopupService.ts'
import { assignedRolesStore } from '@/components/admin/players/assignedRoles/stores/assignedRolesStore.ts'

const assignedUserData = assignedRolesStore()
const popup = usrRoleAssignmentDialogs()
const confirmation = ConfirmationPopup()

const props = defineProps({
  userId: {
    type: String,
    required: true,
  },
})

onBeforeMount(async () => {
  await assignedUserData.getAssignedRoles(props.userId)
})

</script>

<template>
  <div class="text-right">
    <Button label="Add Role" class="m-2" @click="popup.showAddRolePopup()" />
  </div>
  <DataTable :value="assignedUserData.userRoles">
    <Column field="name" header="Role" />
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
        <Button label="Remove" severity="danger" class="m-2 float-end" @click="confirmation.removeUserConfirmation($event, data.roleId, props.userId, data.name)" />
      </template>
    </Column>
  </DataTable>
</template>
