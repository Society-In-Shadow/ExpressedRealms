<script setup lang="ts">

import ToggleSwitch from 'primevue/toggleswitch'
import { onMounted, ref } from 'vue'
import SkeletonWrapper from '@/FormWrappers/SkeletonWrapper.vue'
import type { RoleInfo } from '@/components/admin/players/types'
import { fetchUserPolicies, updateRole } from '@/components/admin/players/services/playerRoleService'
import { userPermissionStore } from '@/stores/userPermissionStore.ts'

const userPermissionData = userPermissionStore()
const permissionCheck = userPermissionData.permissionCheck

const roles = ref<Array<RoleInfo>>([{}, {}])
const isLoading = ref(true)

const emit = defineEmits<{
  policiesChanged: []
}>()

const props = defineProps({
  userId: {
    type: String,
    required: true,
  },
})

onMounted(async () => {
  await fetchUserPolicies(props.userId)
    .then((response) => {
      roles.value = response.data.roles
      isLoading.value = false
    })
})

function roleToggled(roleName: string, isEnabled: boolean) {
  updateRole(props.userId, roleName, isEnabled)
    .then(() => {
      emit('policiesChanged')
    })
}

</script>

<template>
  <div v-for="role in roles" :key="role.name" class="p-2">
    <SkeletonWrapper :show-skeleton="isLoading" height="3rem" width="20em">
      <div class="d-flex d-flex-column">
        <div class="align-self-center">
          <ToggleSwitch v-model="role.isEnabled" :disabled="!permissionCheck.Player.ManageRoles" @change="roleToggled(role.name, role.isEnabled)" />
        </div>
        <div class="align-self-center p-3">
          {{ role.name }}
        </div>
      </div>
    </SkeletonWrapper>
  </div>
</template>
