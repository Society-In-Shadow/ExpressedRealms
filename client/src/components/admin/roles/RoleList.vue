<script setup lang="ts">

import { RoleStore } from './stores/roleStore'
import { onBeforeMount, ref } from 'vue'
import { UserRoles, userStore } from '@/stores/userStore'
import Button from 'primevue/button'
import Card from 'primevue/card'
import RoleItem from '@/components/admin/roles/RoleItem.vue'
import AddRoleForm from './AddRoleForm.vue'

const store = RoleStore()
const userInfo = userStore()
const hasEventManagementRole = ref(false)

onBeforeMount(async () => {
  await store.getRoles()
  hasEventManagementRole.value = await userInfo.hasUserRole(UserRoles.ManageEventRole)
})

const showAdd = ref(false)

const toggleAdd = () => {
  showAdd.value = !showAdd.value
}

const props = defineProps({
  isReadOnly: {
    type: Boolean,
    required: true,
  },
})

</script>

<template>
  <div v-if="store.roles.length == 0" class="text-center">
    <Card>
      <template #title>
        No Roles
      </template>
      <template #content>
        Add One to get Started!
      </template>
    </Card>
  </div>
  <div v-for="role in store.roles" :key="role.id" class="py-3">
    <RoleItem :role="role" :is-read-only="props.isReadOnly" />
  </div>

  <AddRoleForm v-if="showAdd && hasEventManagementRole && !props.isReadOnly" @canceled="toggleAdd" />
  <Button
    v-if="!showAdd && hasEventManagementRole && !props.isReadOnly" class="w-100 m-2"
    label="Add Role" @click="toggleAdd"
  />
</template>
