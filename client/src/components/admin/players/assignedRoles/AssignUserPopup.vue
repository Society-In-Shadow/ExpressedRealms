<script setup lang="ts">

import FormWrapper from '@/FormWrappers/FormWrapper.vue'
import Button from 'primevue/button'
import { inject, onBeforeMount, type Ref } from 'vue'
import FormDropdownWrapper from '@/FormWrappers/FormDropdownWrapper.vue'
import { useRoute } from 'vue-router'
import { getValidationInstance } from '@/components/admin/players/assignedRoles/validation/assignedUserValidation.ts'
import FormInputDateOnlyWrapper from '@/FormWrappers/FormInputDateOnlyWrapper.vue'
import { assignedRolesStore } from '@/components/admin/players/assignedRoles/stores/assignedRolesStore.ts'

const assignedUserData = assignedRolesStore()
const form = getValidationInstance()
const dialogRef = inject('dialogRef') as Ref
const route = useRoute()
const userId = route.params.id as string

onBeforeMount(async () => {
  await assignedUserData.getRoleList()
})

const onSubmit = form.handleSubmit(async (values) => {
  await assignedUserData.addRoleToUser(userId, values)
  dialogRef.value.close()
})

</script>

<template>
  <FormWrapper @submit="onSubmit">
    <FormInputDateOnlyWrapper v-model="form.fields.expireDate" />
    <FormDropdownWrapper v-model="form.fields.role" :options="assignedUserData.roleList" option-label="name" />

    <div class="m-3 text-right">
      <Button label="Update" class="m-2" type="submit" />
    </div>
  </FormWrapper>
</template>
