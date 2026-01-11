<script setup lang="ts">

import { onMounted, ref } from 'vue'
import { RoleStore } from './stores/roleStore'
import FormInputTextWrapper from '@/FormWrappers/FormInputTextWrapper.vue'
import { getValidationInstance } from './validations/roleValidations'
import Button from 'primevue/button'
import FormWrapper from '@/FormWrappers/FormWrapper.vue'
import type { EditRole } from '@/components/admin/roles/types.ts'

const store = RoleStore()

const form = getValidationInstance()
const role = ref<EditRole>()

const props = defineProps({
  roleId: {
    type: Number,
    required: true,
  },
})

onMounted(async () => {
  role.value = await store.getEvent(props.roleId)
  form.setValues(role.value)
})

const onSubmit = form.handleSubmit(async (values) => {
  await store.updateEvent(values, props.roleId)
})

</script>

<template>
  <FormWrapper :show-skeleton="!role" @submit="onSubmit">
    <FormInputTextWrapper v-model="form.fields.name" />
    <FormInputTextWrapper v-model="form.fields.description" />

    <div class="m-3 text-right">
      <Button label="Update" class="m-2" type="submit" />
    </div>
  </FormWrapper>
</template>
