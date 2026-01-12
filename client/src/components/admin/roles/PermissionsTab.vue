<script setup lang="ts">

import { reactive, watch } from 'vue'
import type { Resource } from '@/components/admin/roles/types.ts'
import { RoleStore } from '@/components/admin/roles/stores/roleStore.ts'
import Button from 'primevue/button'
import Panel from 'primevue/panel'
import Checkbox from 'primevue/checkbox'

const store = RoleStore()

const selectedPermissions = reactive<string[]>([])
watch(() => store.role, (role) => {
  if (!role) return

  selectedPermissions.push(...(store.role.permissionIds ?? []))
},
{ immediate: true },
)

const roleData = RoleStore()

const save = async () => {
  roleData.role.permissionIds = selectedPermissions
  await roleData.updateEvent()
}

// Check if a permission is selected
const isPermissionSelected = (id: string) => selectedPermissions.includes(id)

// Toggle permission selection
const togglePermission = (id: string) => {
  const index = selectedPermissions.indexOf(id)
  if (index >= 0) {
    selectedPermissions.splice(index, 1)
  }
  else {
    selectedPermissions.push(id)
  }
}

// Check if all permissions in a resource are selected
const areAllPermissionsSelected = (resource: Resource) =>
  resource.permissions.every(p => selectedPermissions.includes(p.id))

const isSomePermissionsSelected = (resource: Resource) => {
  const selectedCount = resource.permissions.filter(p =>
    selectedPermissions.includes(p.id),
  ).length
  return selectedCount > 0 && selectedCount < resource.permissions.length
}

const toggleResourcePermissions = (resource: Resource) => {
  if (areAllPermissionsSelected(resource)) {
    // Uncheck all permissions
    resource.permissions.forEach((p) => {
      const index = selectedPermissions.indexOf(p.id)
      if (index >= 0) selectedPermissions.splice(index, 1)
    })
  }
  else {
    // Check all permissions
    resource.permissions.forEach((p) => {
      if (!selectedPermissions.includes(p.id)) selectedPermissions.push(p.id)
    })
  }
}

</script>

<template>
  <div class="m-3 text-right">
    <Button label="Update" class="m-2" @click="save" />
  </div>
  <div v-if="roleData.resources && roleData.haveRole" class="resource-grid">
    <Panel v-for="resource in roleData.resources" :key="resource.id">
      <template #header>
        <div class="d-flex justify-content-between align-items-center resource-header flex-grow-1">
          <h3 class="m-0 p-0">
            <label :for="resource.id" class="pl-2">{{ resource.name }}</label>
          </h3>
          <Checkbox
            :input-id="resource.id"
            :model-value="areAllPermissionsSelected(resource)"
            :indeterminate="isSomePermissionsSelected(resource)"
            binary
            @update:model-value="toggleResourcePermissions(resource)"
          />
        </div>
      </template>
      <div class="permissions-list ms-3 mt-2 d-flex flex-column gap-2 ">
        <span
          v-for="permission in resource.permissions"
          :key="permission.id"
          class="d-flex align-items-center gap-1"
        >
          <Checkbox
            :input-id="permission.id"
            :model-value="isPermissionSelected(permission.id)"
            binary
            :aria-label="permission.name"
            @update:model-value="togglePermission(permission.id)"
          />
          <label :for="permission.id" class="pl-2">{{ permission.name }}</label>

          <span
            v-if="permission.description"
            v-tooltip.bottom="permission.description"
            class="material-symbols-outlined"
            :title="permission.description"
          >
            info
          </span>
        </span>
      </div>
    </Panel>
  </div>
</template>

<style scoped>

/* Multi-column responsive grid */
.resource-grid {
  display: grid;
  gap: 1rem; /* space between resource blocks */
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
}

</style>
