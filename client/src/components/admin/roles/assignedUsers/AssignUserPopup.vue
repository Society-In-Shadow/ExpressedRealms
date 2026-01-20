<script setup lang="ts">

import FormWrapper from '@/FormWrappers/FormWrapper.vue'
import Button from 'primevue/button'
import { inject, onBeforeMount, type Ref } from 'vue'
import FormDropdownWrapper from '@/FormWrappers/FormDropdownWrapper.vue'
import { useRoute } from 'vue-router'
import { assignedUsersStore } from '@/components/admin/roles/assignedUsers/stores/assignedUsersStore.ts'
import { getValidationInstance } from '@/components/admin/roles/assignedUsers/validation/assignedUserValidation.ts'
import FormInputDateOnlyWrapper from '@/FormWrappers/FormInputDateOnlyWrapper.vue'

const assignedUserData = assignedUsersStore()
const form = getValidationInstance()
const dialogRef = inject('dialogRef') as Ref
const route = useRoute()
const roleId = Number.parseInt(route.params.id as string)

onBeforeMount(async () => {
  await assignedUserData.getUserList()
})

const onSubmit = form.handleSubmit(async (values) => {
  await assignedUserData.addUserToRole(roleId, values)
  dialogRef.value.close()
})

</script>

<template>
  <FormWrapper @submit="onSubmit">
    <FormInputDateOnlyWrapper v-model="form.fields.expireDate" />
    <FormDropdownWrapper v-model="form.fields.user" :options="assignedUserData.userList" option-label="name" />

    <div class="m-3 text-right">
      <Button label="Update" class="m-2" type="submit" />
    </div>
  </FormWrapper>
</template>
